using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionMenu : MonoBehaviour {

    public GameObject midCamera;
    public GameObject player;
    public bool isMenuEnabled = false;
    // Use this for initialization
    void Start () {
      
    }

    // Update is called once per frame
    void Update () {
        if(isMenuEnabled)
        {
            // user cant move
            player.GetComponent<OVRPlayerController>().GravityModifier = 0.0f;
            // check if instruction is within the camera view
            Vector3 screenPoint = midCamera.GetComponent<Camera>().WorldToViewportPoint(transform.position);
            bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 /*&& screenPoint.y > 0 && screenPoint.y < 1*/;
            Debug.Log(player.transform.forward);

            //if not in view 
            if (!onScreen)
            {
                // user can move nad menu is disable
                gameObject.transform.position = player.transform.forward * 5.0f + player.transform.position;
                gameObject.transform.rotation = Quaternion.LookRotation(gameObject.transform.position - player.transform.position);
            }
        }
    }
}
