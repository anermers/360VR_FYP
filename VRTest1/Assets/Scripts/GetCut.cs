using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCut : MonoBehaviour {

    public Transform rightHand;
    public Transform knife;

	// Use this for initialization
	void Start () {
        knife.position = rightHand.position;
        knife.eulerAngles = new Vector3(180, 90, 180);
        knife.parent = rightHand;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void DropKnife()
    {
        if (knife.GetComponent<Rigidbody>() == null)
            knife.gameObject.AddComponent<Rigidbody>();
        if (knife.GetComponent<BoxCollider>() == null)
            knife.gameObject.AddComponent<BoxCollider>();

        knife.parent = null;
        //disable the script as it is no longer in use
        this.enabled = false;
    }


}
