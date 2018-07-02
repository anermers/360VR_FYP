﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{
    public List<GameObject> buttonsList;

    public GameObject point;
    private float speed;
    private bool choosen;
    public static bool isMoving;
    // Use this for initialization
    void Start()
    {
        speed = 1;
        isMoving = false;
        choosen = false;
        point = null;
        //ChooseScenario("");
    }
    // Update is called once per frame
    void Update()
    {
        if (point != null)
        {
            if (Vector3.Distance(transform.position, point.transform.position) >= 0.5f)
            {
                transform.position += (point.transform.position - transform.position) * speed * Time.deltaTime;
                GetComponent<OVRPlayerController>().enabled = false;
                isMoving = true;
            }
            else
            {
                GetComponent<OVRPlayerController>().enabled = true;
                point = null;
                isMoving = false;
            }
        }
    }
    public void ChooseScenario(string _choice)
    {
        if (!choosen)
        {
            foreach (GameObject iter in buttonsList)
            {
                if (_choice.Equals(iter.name))
                {
                    point = iter;
                    choosen = true;
                }
                else
                    iter.transform.parent.gameObject.SetActive(false);
            }
        }
    }
}
