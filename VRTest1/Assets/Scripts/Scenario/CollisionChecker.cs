using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionChecker : MonoBehaviour {
    private bool correctOBJ;
	// Use this for initialization
	void Start () {
		
	}
	
    private void OnCollisionEnter(Collision collision)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (ScenarioHandler.instance.CurrScenario == null)
            return;

        // SCENARIO_FIRE
        if(ScenarioHandler.instance.CurrScenario.name == "sf")
        {
            if (other.name == "FireBlanket" && gameObject.name == "FIreTriggerbox")
            {
                other.transform.parent = null;
                other.tag = "Untagged";
                other.transform.position = gameObject.transform.position;
                other.transform.eulerAngles = new Vector3(0, 90, 90);
                if(other.GetComponent<Animator>() != null)
                    other.GetComponent<Animator>().SetBool("Play", true);
                ScenarioHandler.instance.CurrScenario.IsEventCompleted = true;
            }
            else
            {
                Debug.Log("other name:" + other.name);
                Debug.Log("this go name:" + gameObject.name);
            }
        }

        // SCENARIO_CUT
        else if (ScenarioHandler.instance.CurrScenario.name == "sc")
        {
            if(gameObject.name == "MedKitTriggerLocal")
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
                    ScenarioHandler.instance.CurrScenario.IsEventCompleted = true;
                }
            }
            else if(gameObject.name == "Chef")
            {
                correctOBJ = false;
                Debug.Log(ScenarioHandler.instance.CurrScenario.GetComponent<ScenarioCut>().currState);
                switch (ScenarioHandler.instance.CurrScenario.GetComponent<ScenarioCut>().currState)
                {
                    case ScenarioCut.STATE_SC.STATE_PURIFIED_WATER:
                        if (other.name == "Acriflavine Solution")
                            correctOBJ = true;
                        break;
                    case ScenarioCut.STATE_SC.STATE_APPLY_GAUZE:
                        if (other.name == "Gauze")
                            correctOBJ = true;
                        break;
                    case ScenarioCut.STATE_SC.STATE_APPLY_BANDANGE:
                        if (other.name == "OmniPlast")
                            correctOBJ = true;
                        break;
                }
                if (correctOBJ)
                {
                    other.gameObject.SetActive(false);
                    ScenarioHandler.instance.CurrScenario.IsEventCompleted = true;
                }
            }
        }

        // SCENARIO_BURN
        else if (ScenarioHandler.instance.CurrScenario.name == "sb")
        {
            if (gameObject.name == "MedKitTriggerLocal")
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
                    ScenarioHandler.instance.CurrScenario.IsEventCompleted = true;
                }
            }
            else if (gameObject.name == "Chef")
            {
                correctOBJ = false;
                Debug.Log(ScenarioHandler.instance.CurrScenario.GetComponent<ScenarioBurn>().currState);
                switch (ScenarioHandler.instance.CurrScenario.GetComponent<ScenarioBurn>().currState)
                {
                    case ScenarioBurn.STATE_SB.STATE_PURIFIED_WATER:
                        if (other.name == "Acriflavine Solution")
                            correctOBJ = true;
                        break;
                    case ScenarioBurn.STATE_SB.STATE_APPLY_BANDANGE:
                        if (other.name == "OmniPlast")
                            correctOBJ = true;
                        break;
                }

                if (correctOBJ)
                {
                    other.gameObject.SetActive(false);
                    ScenarioHandler.instance.CurrScenario.IsEventCompleted = true;
                }
            }
        }
    }

}
