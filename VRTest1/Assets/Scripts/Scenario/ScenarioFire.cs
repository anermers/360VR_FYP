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
        STATE_FIRE_BLANKET,
        STATE_PULL_ALARM,
        STATE_EVACUATE,
        STATE_TOTAL
    }

    public List<SFInfo> sfInfoList;

    STATE_SF currState;
    STATE_SF prevState;
    bool isBigFire = false;
    int instructionIndex;

    Dictionary<STATE_SF, SFInfo> sfInfoContainer; 

	// Use this for initialization
	void Start () {
        //set the 1st state
        sfInfoContainer = new Dictionary<STATE_SF, SFInfo>();
        foreach(SFInfo info in sfInfoList)
        {
            if(!sfInfoContainer.ContainsKey(info.state))
            {
                sfInfoContainer.Add(info.state, info);
            }
        }
        currState = STATE_SF.STATE_FIRE_START;
        prevState = currState;
        isEventCompleted = false;
        isInteracted = false;
        isScenarioDone = false;
        instructionIndex = 0;
        int rand = Random.Range(0, 1);
        if (rand == 0)
            isBigFire = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
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
            case STATE_SF.STATE_FIRE_START:
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
                        SwitchState((int)STATE_SF.STATE_FIRE_BLANKET);
                }
                break;
            case STATE_SF.STATE_FIRE_BLANKET:

                if(isInteracted)
                {
                    //Spawn fire blanket
                    //Bring to fire
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
        ScenarioHandler.instance.interactableGO.Clear();
        foreach (GameObject go in sfInfoContainer[currState].interactables)
            ScenarioHandler.instance.interactableGO.Add(go);
    }
}

