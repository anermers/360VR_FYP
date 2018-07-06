using System.Collections;
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
        isMoving = true;
        choosen = false;
        point = null;
        //ChooseScenario("");
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.V))
            ChooseScenario("Bfire");

        if (point != null)
        {
            if (Vector3.Distance(transform.position, point.transform.position) >= 1)
            {
                point.transform.parent.position += (transform.position - point.transform.position) * speed * Time.deltaTime;
                GetComponent<OVRPlayerController>().enabled = false;
                isMoving = true;
            }
            else
            {
                point = null;
                isMoving = false;
            }
        }
        if(!isMoving)
            GetComponent<OVRPlayerController>().enabled = true;
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
