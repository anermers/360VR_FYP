using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public struct SFInfo
{
    public ScenarioFire.STATE_SF state;
    public List<GameObject> interactables;
    public List<string> instructions;
}

public class ScenarioFire : ScenarioBase
{
    public enum STATE_SF
    {
        STATE_FIRE_START = 0,
        STATE_OFF_GAS,
        STATE_OFF_MAIN_GAS,
        STATE_GET_FIRE_BLANKET,
        STATE_USE_FIRE_BLANKET,
        STATE_PULL_ALARM,
        STATE_EVACUATE,
        STATE_TOTAL
    }

    public GameObject fireBlanket;
    public GameObject smallFire;
    public GameObject largeFire;
    public List<SFInfo> sfInfoList;

    STATE_SF currState;
    STATE_SF prevState;
    bool isBigFire = true;
    int instructionIndex;

    Dictionary<STATE_SF, SFInfo> sfInfoContainer; 
    
	// Use this for initialization
	void Start () {

        //sfInfoContainer = new Dictionary<STATE_SF, SFInfo>();
        //foreach(SFInfo info in sfInfoList)
        //{
        //    if(!sfInfoContainer.ContainsKey(info.state))
        //    {
        //        foreach(GameObject go in info.interactables)
        //        {
        //            if (go.GetComponent<Renderer>() != null &&
        //                go.GetComponent<cakeslice.Outline>() == null)
        //            {
        //                go.AddComponent<cakeslice.Outline>();
        //                go.GetComponent<cakeslice.Outline>().eraseRenderer = true;
        //            }
        //        }
        //        sfInfoContainer.Add(info.state, info);
        //    }
        //}

        Debug.Log("sf_start");
        //set the 1st state
        currState = STATE_SF.STATE_FIRE_START;
        prevState = currState;
        isEventCompleted = false;
        isInteracted = false;
        isScenarioDone = false;
        instructionIndex = 0;
        int rand = Random.Range(0, 2);
        if (rand == 0)
            isBigFire = false;

#if UNITY_EDITOR
        isBigFire = false;
#endif

        smallFire.SetActive(!isBigFire);
        largeFire.SetActive(isBigFire);


    }

    public override void Init()
    {
        Debug.Log("ScenarioFire - Init");
        sfInfoContainer = new Dictionary<STATE_SF, SFInfo>();
        foreach (SFInfo info in sfInfoList)
        {
            if (!sfInfoContainer.ContainsKey(info.state))
            {
                // Adds outline component to each GO in the scenario
                foreach (GameObject go in info.interactables)
                {
                    // If render exist and outline component does not exist
                    if (go.GetComponent<Renderer>() != null &&
                        go.GetComponent<cakeslice.Outline>() == null)
                    {
                        go.AddComponent<cakeslice.Outline>();
                        go.GetComponent<cakeslice.Outline>().color = 0;
                        go.GetComponent<cakeslice.Outline>().eraseRenderer = true;
                    }
                }
                // Adds to the dictionary
                sfInfoContainer.Add(info.state, info);
            }
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if(isScenarioDone)
        {
            Debug.Log("Scenario Completed");
        }

        if(prevState != currState)
        {
            Debug.Log("StateChanged");
            isEventCompleted = false;
            isInteracted = false;
            SetCurrentInteractable();
            SetInstruction();
            prevState = currState;
            //reset index for instructions
            instructionIndex = 0;
        }

        switch(currState)
        {
            case STATE_SF.STATE_FIRE_START: // Buffer state
                SwitchState((int)STATE_SF.STATE_OFF_GAS);
                break;
            case STATE_SF.STATE_OFF_GAS:
                isEventCompleted = isInteracted;
                if(isEventCompleted)
                    SwitchState((int)STATE_SF.STATE_OFF_MAIN_GAS);
                break;
            case STATE_SF.STATE_OFF_MAIN_GAS:
                isEventCompleted = isInteracted;
                if (isEventCompleted)
                {
                    if(isBigFire)
                        SwitchState((int)STATE_SF.STATE_PULL_ALARM);
                    else
                        SwitchState((int)STATE_SF.STATE_GET_FIRE_BLANKET);
                }
                break;
            case STATE_SF.STATE_GET_FIRE_BLANKET:

                if(isInteracted)
                {
                    fireBlanket.SetActive(true);

                    if (InteractedGO == fireBlanket)
                        isEventCompleted = true;
                }

                if (isEventCompleted)
                    SwitchState((int)STATE_SF.STATE_USE_FIRE_BLANKET);
                break;
            case STATE_SF.STATE_USE_FIRE_BLANKET:

                if (InteractedGO != fireBlanket)
                {
                    Debug.Log("Pick up the fire blanket");
                    return;
                }

                if (isEventCompleted)
                    isScenarioDone = true;
                break;
            case STATE_SF.STATE_PULL_ALARM:
                isEventCompleted = isInteracted;
                if (isEventCompleted)
                    SwitchState((int)STATE_SF.STATE_EVACUATE);
                break;
            case STATE_SF.STATE_EVACUATE:
                isEventCompleted = isInteracted;
                if (isEventCompleted)
                    isScenarioDone = true;
                break;
            default:
                break;
        }

    }

    void SetInstruction()
    {
        if (ScenarioHandler.instance.instruction == null
            || sfInfoContainer[currState].instructions.Count <= 0)
            return;
        ScenarioHandler.instance.instruction.text = sfInfoContainer[currState].instructions[instructionIndex];
    }

    protected override bool SwitchState(int index)
    {
        if (index < 0 || index >= (int)STATE_SF.STATE_TOTAL)
            return false;

        prevState = currState;
        currState = (STATE_SF)index;
        return true;
    }

    protected override void SetCurrentInteractable()
    {
        foreach (GameObject go in sfInfoContainer[prevState].interactables)
        {
            if (go.GetComponent<cakeslice.Outline>())
                go.GetComponent<cakeslice.Outline>().eraseRenderer = true;
        }

        ScenarioHandler.instance.interactableGO.Clear();
        foreach (GameObject go in sfInfoContainer[currState].interactables)
        {
            if (go.GetComponent<cakeslice.Outline>())
                go.GetComponent<cakeslice.Outline>().eraseRenderer = false;
            ScenarioHandler.instance.interactableGO.Add(go);
        }
    }

    //IEnumerator FireExtinguish()
    //{
    //}
}

