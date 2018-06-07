using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float speedH = 2.0f;
    public float speedV = 2.0f;
    public float movementSpeed = 10.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    void Update()
    {
        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        
        //if(Input.GetKey(KeyCode.W))
        //    transform.position += transform.forward * movementSpeed * Time.deltaTime;
        //else if(Input.GetKey(KeyCode.S))
        //    transform.position -= transform.forward * movementSpeed * Time.deltaTime;
        //else if(Input.GetKey(KeyCode.A))
        //    transform.position -= transform.right * movementSpeed * Time.deltaTime;
        //else if(Input.GetKey(KeyCode.D))
        //    transform.position += transform.right * movementSpeed * Time.deltaTime;

        //transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z);
    }

}
