using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public static Arrow instance;
    //adjust this to change speed
    float speed = 3f;
    //adjust this to change how high it goes
    float height = 0.3f;
    //gameObject where the arrow should snap to
    public GameObject objectToSnap;
    //Player Camera
    public GameObject playerCamera;

    private void Awake()
    {
        if (!instance)
            instance = this;
    }
    void Update()
    {
        if (objectToSnap != null)
        {
            //Set active if there is a target
            gameObject.SetActive(true);

            //Set to always look at player
            //transform.eulerAngles = new Vector3(transform.rotation.x, , transform.rotation.z);

            //Set position to where the objectToSnap location is
            transform.position = new Vector3(objectToSnap.transform.position.x, objectToSnap.transform.position.y, objectToSnap.transform.position.z);

            //calculate what the new Y position will be
            float newY = Mathf.Sin(Time.time * speed);

            //set the object's Y to the new calculated Y
            transform.position = new Vector3(transform.position.x, newY * height + 2.5f, transform.position.z);
        }
        else
            //Set false if there is no target
            gameObject.SetActive(false);
    }
}
