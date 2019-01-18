using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowfieldParticle : MonoBehaviour {

    public int audioband;
    private int band;

    public float moveSpeed;
    public int moveTime;

    public int instantiateAmount;
    public GameObject flowFieldParticle;

    private NoiseFlowField NFF;
    private AudioFlowField AFF;
    public AudioPeer audioPeer;

 //   // Use this for initialization
 //   void Awake () {
 //       NFF = GameObject.Find("NoiseFlowField").GetComponent<NoiseFlowField>();
 //       audioPeer = GameObject.Find("AudioPeer").GetComponent<AudioPeer>();
 //       AFF = NFF.GetComponent<AudioFlowField>();

 //       //this.gameObject.GetComponent<MeshRenderer>().material = AFF.audioMaterial[band];
 //       //audioband = band;
 //       //this.transform.rotation = Random.rotation;

 //       StartCoroutine(SpawnAndDestroy());
	//}
	
	// Update is called once per frame
	void Update () {
        this.transform.position += transform.forward * moveSpeed * Time.deltaTime;
	}

    public void ApplyRotation(Vector3 rotation, float rotationSpeed)
    {
        Quaternion targetRotation = Quaternion.LookRotation(rotation.normalized);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    //IEnumerator SpawnAndDestroy()
    //{
    //    yield return new WaitForSeconds(moveTime);
    //    NFF.particles.Remove(this.gameObject.GetComponent<FlowfieldParticle>());
    //    NFF.particleMeshRenderer.Remove(this.gameObject.GetComponent<MeshRenderer>());
    //    NFF.particleLocations.Remove(this.gameObject.GetComponent<Transform>());
    //    NFF.updateParticleList();

    //    for(int i = 0; i < instantiateAmount; i++)
    //    {
    //        GameObject tempHolder = (GameObject)Instantiate(flowFieldParticle);
    //        tempHolder.transform.position = this.transform.position;
    //        tempHolder.transform.rotation = Random.rotation;

    //        tempHolder.transform.parent = NFF.transform;
    //        tempHolder.transform.localScale = new Vector3(NFF.particleScale, NFF.particleScale, NFF.particleScale);

    //        NFF.particles.Add(tempHolder.GetComponent<FlowfieldParticle>());
    //        NFF.particleMeshRenderer.Add(tempHolder.GetComponent<MeshRenderer>());
    //        NFF.particleLocations.Add(tempHolder.GetComponent<Transform>());

    //    }
    //    Destroy(this.gameObject);
    //}
}
