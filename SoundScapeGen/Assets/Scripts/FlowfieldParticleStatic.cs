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

    public Vector2 rotateSpeedMinMax;

    private float counter = 0;
    public float waitTime;

    public float rotateSpeed;

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
    }
}
