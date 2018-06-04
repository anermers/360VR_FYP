using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Test : MonoBehaviour {
    Mesh mesh;
	// Use this for initialization
	void Start () {
        mesh = GetComponent<MeshFilter>().mesh;
        mesh.triangles = mesh.triangles.Reverse().ToArray();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
