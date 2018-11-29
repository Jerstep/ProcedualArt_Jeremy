using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateWeardness : MonoBehaviour {

    public GameObject _prefabGameObject;
    List<GameObject> _sampleGameObjects = new List<GameObject>();

    bool _startMadness;

    private IEnumerator coroutine;

    // Use this for initialization
    void Start () {
        coroutine = Wait(0.15f);
        StartCoroutine(coroutine);
    }
	
	// Update is called once per frame
	void Update () {
        if(_startMadness)
        {
            ObjectMadness();
        }
	}

    private IEnumerator Wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        _startMadness = true;
    }

    void ObjectMadness()
    {

    }
}
