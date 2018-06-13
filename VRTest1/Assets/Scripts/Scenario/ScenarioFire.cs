using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    Dictionary<STATE_SF, SFInfo> sfInfoContainer; 

	// Use this for initialization
	void Start () {
        //set the 1st state
        currState = STATE_SF.STATE_FIRE_START;
        prevState = currState;
        isEventCompleted = false;
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
            OnEnterState();
        }

        switch(currState)
        {
            case STATE_SF.STATE_FIRE_START:
                SwitchState((int)STATE_SF.STATE_OFF_GAS);
                break;
            case STATE_SF.STATE_OFF_GAS:

                if(isEventCompleted)
                    SwitchState((int)STATE_SF.STATE_OFF_MAIN_GAS);
                break;
            case STATE_SF.STATE_OFF_MAIN_GAS:

                if (isEventCompleted)
                {
                    if(isBigFire)
                        SwitchState((int)STATE_SF.STATE_PULL_ALARM);
                    else
                        SwitchState((int)STATE_SF.STATE_FIRE_BLANKET);
                }
                break;
            case STATE_SF.STATE_FIRE_BLANKET:

                if (isEventCompleted)
                    isScenarioDone = true;
                break;
            case STATE_SF.STATE_PULL_ALARM:

                if(isEventCompleted)
                    SwitchState((int)STATE_SF.STATE_EVACUATE);
                break;
            case STATE_SF.STATE_EVACUATE:

                if (isEventCompleted)
                    isScenarioDone = true;
                break;
            default:
                break;
        }

    }

    protected override bool SwitchState(int index)
    {
        if (index < 0 || index >= (int)STATE_SF.STATE_TOTAL)
            return false;

        prevState = currState;
        currState = (STATE_SF)index;
        return true;
    }

    protected override void OnEnterState()
    {
        switch (currState)
        {
            case STATE_SF.STATE_FIRE_START:

                break;
            case STATE_SF.STATE_OFF_GAS:
                break;
            case STATE_SF.STATE_OFF_MAIN_GAS:
                break;
            case STATE_SF.STATE_FIRE_BLANKET:
                break;
            case STATE_SF.STATE_PULL_ALARM:
                break;
            case STATE_SF.STATE_EVACUATE:
                break;
            default:
                break;
        }

    }
}

