using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour {

    public Vector2 rotateSpeedMinMax;
    private float rotateSpeed;

    public AudioPeer audioPeer;

    public bool up;
    public bool right;
    public bool left;
    public bool back;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {

        rotateSpeed = Mathf.Lerp(rotateSpeedMinMax.x, rotateSpeedMinMax.y, audioPeer._AmplitudeBuffer);

        if(up)
        {
            this.transform.Rotate(Vector3.up * (Time.deltaTime * rotateSpeed));
        }

        if(left)
        {
            this.transform.Rotate(Vector3.left * (Time.deltaTime * rotateSpeed));
        }

        if(right)
        {
            this.transform.Rotate(Vector3.right * (Time.deltaTime * rotateSpeed));
        }

        if(back)
        {
            this.transform.Rotate(Vector3.back * (Time.deltaTime * rotateSpeed));
        }
        //this.transform.Rotate(Vector3.right * (Time.deltaTime * rotateSpeed));
    }
}
