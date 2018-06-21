using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAway : MonoBehaviour {

    public GameObject target;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		if(target)
        {
            transform.LookAt(target.transform);
            transform.Rotate(0, 90, 0);
            transform.position -= transform.right * Time.deltaTime;
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
	}
}
