using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody>().detectCollisions = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.I))
            GetComponent<Rigidbody>().detectCollisions = !GetComponent<Rigidbody>().detectCollisions;
    }
}
