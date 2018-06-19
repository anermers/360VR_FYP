using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionChecker : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
    private void OnCollisionEnter(Collision collision)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (ScenarioHandler.instance.CurrScenario == "sf")
        {
            if (other.name == "FireBlanket")
            {
                other.transform.parent = null;
                other.tag = "Untagged";
                other.transform.position = gameObject.transform.position;
                other.transform.Rotate(0, 90, 90);
                if (other.GetComponent<Animator>() != null)
                    other.GetComponent<Animator>().SetBool("Play", true);
                ScenarioHandler.instance.ScenarioContainer[ScenarioHandler.instance.CurrScenario].IsEventCompleted = true;
            }
            else
            {
                Debug.Log("other name:" + other.name);
                Debug.Log("this go name:" + gameObject.name);
            }
        }

        else if (ScenarioHandler.instance.CurrScenario == "sc")
        {
            if (other.name == "MedKit")
            {
                Debug.Log(other.name);
                other.transform.parent = null;
                other.tag = "Untagged";
                other.transform.position = gameObject.transform.position;
                other.transform.eulerAngles = new Vector3(0, -90, 0);
                if (other.GetComponent<Animator>() != null)
                    other.GetComponent<Animator>().SetBool("Play", true);

                other.GetComponent<Rigidbody>().useGravity = false;
                other.GetComponent<Rigidbody>().detectCollisions = false;
                other.GetComponent<BoxCollider>().enabled = false;
                ScenarioHandler.instance.ScenarioContainer[ScenarioHandler.instance.CurrScenario].IsEventCompleted = true;
            }
        }
    }

}
