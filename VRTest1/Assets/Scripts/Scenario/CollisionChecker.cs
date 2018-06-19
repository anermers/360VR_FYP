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
<<<<<<< HEAD
        if (ScenarioHandler.instance.CurrScenario == "sf")
=======
        if(ScenarioHandler.instance.CurrScenario.name == "sf")
>>>>>>> 965d1bd4186e00ba8482b70962ed6bc67611f373
        {
            if (other.name == "FireBlanket")
            {
                other.transform.parent = null;
                other.tag = "Untagged";
                other.transform.position = gameObject.transform.position;
<<<<<<< HEAD
                other.transform.Rotate(0, 90, 90);
                if (other.GetComponent<Animator>() != null)
=======
                other.transform.eulerAngles = new Vector3(0, 90, 90);
                if(other.GetComponent<Animator>() != null)
>>>>>>> 965d1bd4186e00ba8482b70962ed6bc67611f373
                    other.GetComponent<Animator>().SetBool("Play", true);
                ScenarioHandler.instance.CurrScenario.IsEventCompleted = true;
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
