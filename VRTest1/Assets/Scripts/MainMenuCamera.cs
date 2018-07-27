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
        isMoving = false;
        choosen = true;
        point = null;
        //ChooseScenario("");

        foreach (GameObject iter in buttonsList)
        {
            if (iter.transform.parent.GetComponent<ButtonFloating>() == null)
                iter.transform.parent.gameObject.AddComponent<ButtonFloating>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.V))
            ChooseScenario("Bfire");

        if (point != null && point.name != "Tutorial")
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
                    iter.transform.parent.GetComponent<ButtonFloating>().enabled = false;
                }
                else
                    iter.transform.parent.gameObject.SetActive(false);
            }
        }
    }
}
