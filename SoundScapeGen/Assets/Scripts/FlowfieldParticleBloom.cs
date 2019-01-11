using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowfieldParticleBloom : MonoBehaviour {

    //public float moveSpeed;
    public int audioband;
    private NoiseFlowField NFF;
    private AudioFlowField AFF;

    private float counter;
    public float waitTime;
    public float moveSpeed;

    // Use this for initialization
    void Start () {
        NFF = GameObject.Find("NoiseFlowField").GetComponent<NoiseFlowField>();
        AFF = NFF.GetComponent<AudioFlowField>();
        int band = Random.Range(0, 8);

        //for(int i = 0; i < NFF.bloomParticles.Count; i++)
        //{
        //    //int band = NFF.countBand % 8;
            
        //    NFF.staticParticleMeshRenderer[i].material = AFF.audioMaterial[band];
        //    NFF.staticParticles[i].audioband = band;

        //    NFF.countBand++;
        //}            
    }

    void Update()
    {
        this.transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    void SpawnStaticParticle()
    {
        if(counter < waitTime)
        {
            counter += Time.deltaTime;
        }
        else
        {
            GameObject particleStaticInstance = (GameObject)Instantiate(NFF.particleStaticPrefab);
            particleStaticInstance.transform.position = this.transform.position;
            particleStaticInstance.transform.parent = this.transform;
            particleStaticInstance.transform.localScale = new Vector3(NFF.particleScale, NFF.particleScale, NFF.particleScale);
            NFF.staticParticles.Add(particleStaticInstance.GetComponent<FlowfieldParticleStatic>());
            NFF.staticParticleMeshRenderer.Add(particleStaticInstance.GetComponent<MeshRenderer>());

            counter = 0;
        }
    }

    public void ApplyRotation(Vector3 rotation, float rotationSpeed)
    {
        Quaternion targetRotation = Quaternion.LookRotation(rotation.normalized);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
