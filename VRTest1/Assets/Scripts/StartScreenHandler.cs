using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenHandler : MonoBehaviour {

    public GameObject loadScreen;
    public GameObject spiningThing;
    public GameObject clickText;
    public GameObject uiCamera;

    //adjust this to change speed
    public float speed = 3f;
    //adjust this to change how high it goes
    public float height = 0.3f;

    // orginal y for text
    private float defaultY = 0;

    // Use this for initialization
    void Start () {
        defaultY = clickText.transform.position.y;

    }
	
	// Update is called once per frame
	void Update () {

        //gameObject.transform.position = uiCamera.transform.forward * 100f + uiCamera.transform.position;
        //gameObject.transform.rotation = Quaternion.LookRotation(gameObject.transform.position - uiCamera.transform.position);

        if (loadScreen.activeSelf)
        {
            spiningThing.transform.Rotate(Vector3.forward * 100.0f * Time.deltaTime);
            return;
        }

        //calculate what the new Y position will be
        float newY = Mathf.Sin(Time.time * speed);
        //set the object's Y to the new calculated Y
        clickText.transform.position = new Vector3(clickText.transform.position.x, clickText.transform.position.y + newY * height, clickText.transform.position.z);
        //clickText.transform.position = new Vector3(clickText.transform.position.x, defaultY + newY * height + 2.5f, clickText.transform.position.z);

        if (OVRInput.Get(OVRInput.Button.PrimaryTouchpad) || OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) ||
            Input.GetKey(KeyCode.F12))
        {
            SceneManager.LoadSceneAsync("main");
            loadScreen.SetActive(true);
        }
	}
}
