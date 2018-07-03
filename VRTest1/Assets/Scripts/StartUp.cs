using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUp : MonoBehaviour {

    public Transform rightHand;
    public Transform dishObj;
    public Transform intialSpawn;
        
    private Animator chefAnim;

    // Use this for initialization
	void Start () {
        // Set animator
        chefAnim = GetComponent<Animator>();
        // Set the pos of the chef to the satrt point
        transform.position = intialSpawn.position;

        //dishObj.position = rightHand.position;
        //dishObj.parent = rightHand;
        chefAnim.SetBool("placeDish", true);
        chefAnim.SetBool("placeDishIdle", true);
    }
	
	// Update is called once per frame
	void Update () {


            // get key input
            if (OVRInput.Get(OVRInput.Button.PrimaryTouchpad) || OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) ||
            Input.GetKey(KeyCode.F12))
        {
            this.enabled = false;
        }

    }

    void DropDish()
    {

    }
}
