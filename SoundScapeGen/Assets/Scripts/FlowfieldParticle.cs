using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowfieldParticle : MonoBehaviour {

    public float moveSpeed;
    public int audioband;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position += transform.forward * moveSpeed * Time.deltaTime;
	}

    public void ApplyRotation(Vector3 rotation, float rotationSpeed)
    {
        Quaternion targetRotation = Quaternion.LookRotation(rotation.normalized);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
