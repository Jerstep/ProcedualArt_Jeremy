using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowfieldParticleStatic : MonoBehaviour {

    //public float moveSpeed;
    public int audioband;
    private NoiseFlowField NFF;
    private AudioFlowField AFF;
    public AudioPeer audioPeer;
    private int band;
    [SerializeField] private float counter = 0;

    public Vector2 rotateSpeedMinMax;

    public float rotateSpeed;
    public int disapperaTime;
    public bool removeAfterTime;

    // Use this for initialization
    private void Awake()
    {
        band = Random.Range(0, 8);
    }

    void Start () {
        NFF = GameObject.Find("NoiseFlowField").GetComponent<NoiseFlowField>();
        audioPeer = GameObject.Find("AudioPeer").GetComponent<AudioPeer>();
        AFF = NFF.GetComponent<AudioFlowField>();
        this.gameObject.GetComponent<MeshRenderer>().material = AFF.audioMaterial[band];
        audioband = band;
        this.transform.rotation = Random.rotation;
    }

    private void Update()
    {
        rotateSpeed = Mathf.Lerp(rotateSpeedMinMax.x, rotateSpeedMinMax.y, audioPeer._AmplitudeBuffer);
        this.transform.Rotate(Vector3.up * (Time.deltaTime * rotateSpeed));
        this.transform.Rotate(Vector3.back * (Time.deltaTime * rotateSpeed));
        
        if(removeAfterTime)
        {
            counter += Time.deltaTime;

            if(counter > disapperaTime)
            {
                NFF.staticParticles.Remove(this.gameObject.GetComponent<FlowfieldParticleStatic>());
                NFF.staticParticleMeshRenderer.Remove(this.gameObject.GetComponent<MeshRenderer>());
                gameObject.SetActive(false);
                Destroy(gameObject);
            }
        }
    }
}
