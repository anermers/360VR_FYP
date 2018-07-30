﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionChecker : MonoBehaviour {
    private bool correctOBJ;
	// Use this for initialization
	void Start () {

        correctOBJ  = false;
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
                //other.transform.position = gameObject.transform.position;
                other.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 0.23f, gameObject.transform.position.z);
                other.transform.eulerAngles = new Vector3(0, 0, 360);
                if(other.GetComponent<Animator>() != null)
                    other.GetComponent<Animator>().SetBool("Play", true);
                ScenarioHandler.instance.CurrScenario.IsEventCompleted = true;
            }
            else if(gameObject.name == "Evac" && other.name == "Chef")
            {
                other.gameObject.SetActive(false);
            }


            if(ScenarioHandler.instance.CurrScenario.GetComponent<ScenarioFire>().currState
                == ScenarioFire.STATE_SF.STATE_EVACUATE)
            {
                if (gameObject.name == "Evac" && other.tag == "Player")
                    ScenarioHandler.instance.CurrScenario.IsInteracted = true;
            }
            else if(ScenarioHandler.instance.CurrScenario.GetComponent<ScenarioFire>().currState
                == ScenarioFire.STATE_SF.STATE_USE_FIRE_EXTINGUISHER)
            {
                Debug.Log(gameObject.name + " : " + other.name);
                if (gameObject.name == "feTriggerbox" && other.name == "FireExtinguisher")
                    ScenarioHandler.instance.CurrScenario.IsEventCompleted = true;
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
            else if(gameObject.name == "Chef" && other.tag == "PickUp")
            {
                correctOBJ = false;
                Debug.Log(ScenarioHandler.instance.CurrScenario.GetComponent<ScenarioCut>().currState);
                switch (ScenarioHandler.instance.CurrScenario.GetComponent<ScenarioCut>().currState)
                {
                    case ScenarioCut.STATE_SC.STATE_PURIFIED_WATER:
                        if (other.name == "EyeWash(Clone)")
                            correctOBJ = true;
                        break;
                    case ScenarioCut.STATE_SC.STATE_APPLY_GAUZE:
                        if (other.name == "Gauze(Clone)")
                            correctOBJ = true;
                        break;
                    case ScenarioCut.STATE_SC.STATE_APPLY_YELLOW_ACRI:
                        if (other.name == "Acriflavine Solution(Clone)")
                            correctOBJ = true;
                        break;
                    case ScenarioCut.STATE_SC.STATE_APPLY_BANDANGE:
                        if (other.name == "OmniPlast(Clone)")
                            correctOBJ = true;
                        break;
                }
                if (correctOBJ)
                {
                    gameObject.GetComponent<GreenParticle>().PlayGreenParticle();
                    other.gameObject.SetActive(false);
                    MedKitUI.Spawn = true;
                    ScenarioHandler.instance.CurrScenario.IsEventCompleted = true;
                }
                else 
                {
                    gameObject.GetComponent<GreenParticle>().PlayRedParticle();
                    other.gameObject.SetActive(false);
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
                    NonUIInteraction.objectSelected = false;
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
            else if (gameObject.name == "Chef" && other.tag =="PickUp")
            {
                correctOBJ = false;
                Debug.Log(ScenarioHandler.instance.CurrScenario.GetComponent<ScenarioBurn>().currState);
                switch (ScenarioHandler.instance.CurrScenario.GetComponent<ScenarioBurn>().currState)
                {
                    case ScenarioBurn.STATE_SB.STATE_PURIFIED_WATER:
                        if (other.name == "EyeWash(Clone)")
                            correctOBJ = true;
                        break;
                    case ScenarioBurn.STATE_SB.STATE_APPLY_CREAM:
                        if (other.name == "Bacidin(Clone)")
                            correctOBJ = true;
                        break;
                    case ScenarioBurn.STATE_SB.STATE_APPLY_BANDANGE:
                        if (other.name == "OmniPlast(Clone)")
                            correctOBJ = true;
                        break;
                }
                if (correctOBJ)
                {
                    gameObject.GetComponent<GreenParticle>().PlayGreenParticle();
                    other.gameObject.SetActive(false);
                    MedKitUI.Spawn = true;
                    ScenarioHandler.instance.CurrScenario.IsEventCompleted = true;
                }
                else 
                {
                    other.gameObject.SetActive(false);
                    gameObject.GetComponent<GreenParticle>().PlayRedParticle();
                }
            }
        }
    }

}
