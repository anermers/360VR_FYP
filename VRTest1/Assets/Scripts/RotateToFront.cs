using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToFront : MonoBehaviour {

    public RayPointer rp;

	// Use this for initialization
	void Start () {
        transform.LookAt(rp.wsPoint + rp.wPoint);
    }
	
    public void Init()
    {
        transform.LookAt(rp.wsPoint + rp.wPoint);
    }
}
