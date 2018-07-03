using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUp : MonoBehaviour {

    //public Transform rightHand;
    //public Transform dishObj;
    public Transform playerCamera;
    public Transform intialSpawn;
    public Transform selectionPoint;
    public Transform startUpPoint;

    public Animator chefAnim;

    public GameObject menuCanvas;

    private static StartUp instance = null;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            DestroyObject(gameObject);

        }
    }
    // Use this for initialization
    void Start () {
        // Set the pos of the chef to the satrt point
        chefAnim.gameObject.transform.position = intialSpawn.position;
        playerCamera.position = startUpPoint.position;
        menuCanvas.SetActive(false);
        //dishObj.position = rightHand.position;
        //dishObj.parent = rightHand;
        chefAnim.SetBool("placeDish", true);
        chefAnim.SetBool("placeDishIdle", true);
    }
	
	// Update is called once per frame
	void Update () {

        if(chefAnim.GetCurrentAnimatorStateInfo(0).IsName("PlaceDishIdle"))
        {
            // get key input
            if (OVRInput.Get(OVRInput.Button.PrimaryTouchpad) || OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) ||
            Input.GetKey(KeyCode.F12))
            {
                menuCanvas.SetActive(true);
                chefAnim.SetBool("placeDish", false);
                playerCamera.position = selectionPoint.position;
                gameObject.SetActive(false);
                this.enabled = false;
            }
        }
    }
}
