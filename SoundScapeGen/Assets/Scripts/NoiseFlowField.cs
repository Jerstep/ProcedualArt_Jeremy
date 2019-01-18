using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseFlowField : MonoBehaviour {

    FastNoise _fastNoise;
    public Vector3Int _gridSize;
    public float _cellSize;
    public float _increment;
    public Vector3 _offset, _offsetSpeed;
    public Vector3[,,] _flowfieldDirection;

    //Particles
    [Header("Particle Prefabs")]
    public GameObject particlePrefab;
    public GameObject particleStaticPrefab;

    [Header("Particle Bounderies (amount)")]
    public int amountParticles;
    public int maxStatic;

    [Header("Particle Branch")]
    public int amountBranchesPerParticle;
    public float particleMoveTime;
    private float branchCounter = 0f;


    //[HideInInspector]
    public List<Transform> particleLocations; 
    public List<FlowfieldParticle> particles;
    public List<MeshRenderer> particleMeshRenderer;

    public List<FlowfieldParticleStatic> staticParticles;
    public List<MeshRenderer> staticParticleMeshRenderer;


    public float particleScale;
    public float spawnRadius, particleMoveSpeed, particleRotateSpeed;
    private float counter = 0f;
    public float waitTime;

    public int countBand = 0;

    private int particleCount;


    bool particleSpawnValidation(Vector3 position)
    {
        bool valid = true;
        foreach (FlowfieldParticle particle in particles)
        {
            if (Vector3.Distance(position, particle.transform.position) < spawnRadius)
            {
                valid = false;
                break;
            }
        }

        if(valid)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Awake()
    {
        StartFlowField();
    }

    // Use this for initialization
    void StartFlowField () {
        _flowfieldDirection = new Vector3[_gridSize.x, _gridSize.y, _gridSize.z];
        _fastNoise = new FastNoise();
        particles = new List<FlowfieldParticle>();
        particleMeshRenderer = new List<MeshRenderer>();

        for(int i = 0; i < amountParticles; i++)
        {
            int attempt = 0;

            while (attempt < 100)
            {
                Vector3 randomPos = new Vector3(
                    Random.Range(this.transform.position.x, this.transform.position.x + _gridSize.x * _cellSize),
                        /*Random.Range(this.transform.position.y, this.transform.position.y + _gridSize.y * _cellSize)*/0,
                            Random.Range(this.transform.position.z, this.transform.position.z + _gridSize.z * _cellSize)
                            );

                bool isValid = particleSpawnValidation(randomPos);

                if(isValid)
                {
                    GameObject particleInstance = (GameObject)Instantiate(particlePrefab);
                    particleInstance.transform.position = randomPos;
                    particleInstance.transform.eulerAngles = new Vector3(Random.Range(-360, 360), Random.Range(-360, 360), Random.Range(-360, 360));
                    particleInstance.transform.parent = this.transform;
                    particleInstance.transform.localScale = new Vector3(particleScale, particleScale, particleScale);
                    particles.Add(particleInstance.GetComponent<FlowfieldParticle>());
                    particleMeshRenderer.Add(particleInstance.GetComponent<MeshRenderer>());
                    particleLocations.Add(particleInstance.GetComponent<Transform>());
                    break;
                }
                if(!isValid)
                {
                    attempt++;
                }
            }
        }
        Debug.Log(particles.Count);
	}
	
    public void updateParticleList()
    {
        particles = new List<FlowfieldParticle>();
        particleMeshRenderer = new List<MeshRenderer>();
        Debug.Log(particles.Count);
    }

	// Update is called once per frame
	void Update () {
        if(staticParticles.Count < maxStatic)
        {
            SpawnStaticParticle();
            if(particles.Count < 64)
            {
                if(branchCounter < particleMoveTime)
                {
                    branchCounter += Time.deltaTime;
                }
                else
                {
                    SpawnBranchParticle();
                    branchCounter = 0;
                }
            }

        }

        CalculateFlowfieldDirections();
        ParticleBehaviour();
    }

    void SpawnStaticParticle()
    {
        if(counter < waitTime)
        {
            counter += Time.deltaTime;
        }
        else
        {
            for(int i = 0; i < particles.Count; i++)
            {                
                GameObject particleStaticInstance = (GameObject)Instantiate(particleStaticPrefab);
                particleStaticInstance.transform.position = particleLocations[i].position;
                particleStaticInstance.transform.parent = this.transform;
                particleStaticInstance.transform.localScale = new Vector3(particleScale, particleScale, particleScale);
                staticParticles.Add(particleStaticInstance.GetComponent<FlowfieldParticleStatic>());
                staticParticleMeshRenderer.Add(particleStaticInstance.GetComponent<MeshRenderer>());
            }

            counter = 0;
        }
    }

    private void SpawnBranchParticle()
    {
        int amountParticles = particles.Count;
        Debug.Log("Amount: " + amountParticles);

        for(int i = 0; i < amountParticles; i++)
        {
            Debug.Log("Ins");
            //particles[i].transform.eulerAngles = new Vector3(Random.Range(-360, 360), Random.Range(-360, 360), Random.Range(-360, 360));

            for(int e = 0; e < amountBranchesPerParticle; e++)
            {
                Debug.Log("Ins");
                GameObject particleInstance = (GameObject)Instantiate(particlePrefab);
                particleInstance.transform.position = particles[i].transform.position;
                particleInstance.transform.eulerAngles = new Vector3(Random.Range(-360, 360), Random.Range(-360, 360), Random.Range(-360, 360));
                particleInstance.transform.parent = this.transform;
                particleInstance.transform.localScale = new Vector3(particleScale, particleScale, particleScale);

                particles.Add(particleInstance.GetComponent<FlowfieldParticle>());
                particleMeshRenderer.Add(particleInstance.GetComponent<MeshRenderer>());
                particleLocations.Add(particleInstance.GetComponent<Transform>());
            }
        }
    }

    void CalculateFlowfieldDirections()
    {
        _fastNoise = new FastNoise();
        _offset = new Vector3(_offset.x + (_offsetSpeed.x * Time.deltaTime), _offset.y + (_offsetSpeed.y * Time.deltaTime), _offset.z + (_offsetSpeed.z * Time.deltaTime));

        float xOff = 0f;
        for(int x = 0; x < _gridSize.x; x++)
        {
            float yOff = 0f;
            for(int y = 0; y < _gridSize.y; y++)
            {
                float zOff = 0f;
                for(int z = 0; z < _gridSize.z; z++)
                {
                    float noise = _fastNoise.GetSimplex(xOff + _offset.x, yOff + _offset.y, zOff + _offset.z) + 1;
                    Vector3 noiseDirection = new Vector3(Mathf.Cos(noise * Mathf.PI), Mathf.Sin(noise * Mathf.PI), Mathf.Cos(noise * Mathf.PI));
                    _flowfieldDirection[x, y, z] = Vector3.Normalize(noiseDirection);
                    zOff += _increment;
                }
                yOff += _increment;
            }
            xOff += _increment;
        }
    }

    private void ParticleBehaviour()
    {
        foreach(FlowfieldParticle p in particles)
        {
            //X Edge
            if (p.transform.position.x > this.transform.position.x + (_gridSize.x * _cellSize))
                p.transform.position = new Vector3(this.transform.position.x, p.transform.position.y, p.transform.position.z);

            if(p.transform.position.x < this.transform.position.x)
                p.transform.position = new Vector3(this.transform.position.x + (_gridSize.x * _cellSize), p.transform.position.y, p.transform.position.z);

            //Y Edge
            if(p.transform.position.y > this.transform.position.y + (_gridSize.y * _cellSize))
                p.transform.position = new Vector3(p.transform.position.x, this.transform.position.y, p.transform.position.z);

            if(p.transform.position.y < this.transform.position.y)
                p.transform.position = new Vector3(p.transform.position.x, this.transform.position.y + (_gridSize.y * _cellSize), p.transform.position.z);

            //Z Edge
            if(p.transform.position.z > this.transform.position.z + (_gridSize.z * _cellSize))
                p.transform.position = new Vector3(p.transform.position.x, p.transform.position.y, this.transform.position.z);

            if(p.transform.position.z < this.transform.position.z)
                p.transform.position = new Vector3(p.transform.position.x, p.transform.position.y, this.transform.position.z + (_gridSize.z * _cellSize));


            Vector3Int particlePos = new Vector3Int(
                Mathf.FloorToInt(Mathf.Clamp((p.transform.position.x - this.transform.position.x) / _cellSize, 0, _gridSize.x - 1)),
                    Mathf.FloorToInt(Mathf.Clamp((p.transform.position.y - this.transform.position.y) / _cellSize, 0, _gridSize.y - 1)),
                        Mathf.FloorToInt(Mathf.Clamp((p.transform.position.z - this.transform.position.z) / _cellSize, 0, _gridSize.z - 1))
                        );

            p.ApplyRotation(_flowfieldDirection[particlePos.x, particlePos.y, particlePos.z], particleRotateSpeed);
            p.moveSpeed = particleMoveSpeed;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(this.transform.position + new Vector3((_gridSize.x * _cellSize) * 0.5f, (_gridSize.y * _cellSize) * 0.5f, (_gridSize.z * _cellSize) * 0.5f),
            new Vector3(_gridSize.x * _cellSize, _gridSize.y * _cellSize, _gridSize.z * _cellSize));
    }
}
