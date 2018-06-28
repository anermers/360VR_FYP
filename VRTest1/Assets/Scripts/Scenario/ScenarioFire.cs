using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using cakeslice;

[System.Serializable]
public struct SFInfo
{
    public ScenarioFire.TYPE_FIRE type;
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

    public enum TYPE_FIRE
    {
        BOTH_FIRE,
        SMALL_FIRE,
        LARGE_FIRE,
    }

    public GameObject traineeChef;
    public GameObject fireBlanket;
    public GameObject smallFire;
    public GameObject largeFire;
    public List<SFInfo> sfInfoList;

    STATE_SF currState;
    STATE_SF prevState;
    bool isBigFire = true;
    int instructionIndex;
    Dictionary<STATE_SF, SFInfo> sfInfoContainer;

    private float timer;
    private Animator chefAnimController;

    private List<string> smallInstructionList;
    private List<string> largeInstructionList;

    // Use this for initialization
    public void Start () {
        Debug.Log("sf_start");

        sfInfoContainer = new Dictionary<STATE_SF, SFInfo>();
        allInstructions = new List<string>();
        smallInstructionList = new List<string>();
        largeInstructionList = new List<string>();

        foreach (SFInfo info in sfInfoList)
        {
            // adds the instructions to allinstruction list
            if (info.instructions.Count > 0)
            {
                //allInstructions.Add(info.instructions[0]);
                switch(info.type)
                {
                    case TYPE_FIRE.BOTH_FIRE:
                        smallInstructionList.Add(info.instructions[0]);
                        largeInstructionList.Add(info.instructions[0]);
                        break;
                    case TYPE_FIRE.LARGE_FIRE:
                        largeInstructionList.Add(info.instructions[0]);
                        break;
                    case TYPE_FIRE.SMALL_FIRE:
                        smallInstructionList.Add(info.instructions[0]);
                        break;
                }
            }


            if (!sfInfoContainer.ContainsKey(info.state))
            {
                // Adds outline component to each GO in the scenario
                foreach (GameObject go in info.interactables)
                {
                    // If renderer exist and outline component does not exist
                    if (go.GetComponent<Renderer>() != null &&
                        go.GetComponent<Outline>() == null)
                    {
                        go.AddComponent<Outline>();
                        go.GetComponent<Outline>().color = 0;
                        go.GetComponent<Outline>().eraseRenderer = false;
                        go.GetComponent<Outline>().enabled = false;
                    }
                }
                // Adds to the dictionary
                sfInfoContainer.Add(info.state, info);
            }
        }
        Debug.Log("COUNT: " + AllInstructions.Count);
        Init();
        ScenarioHandler.instance.instructionScreen.PopulateInsutructionMenu();
    }

    public override void Init()
    {
        Debug.Log("ScenarioFire - Init");

        currState = STATE_SF.STATE_FIRE_START;
        prevState = currState;
        isEventCompleted = false;
        isInteracted = false;
        isScenarioDone = false;
        step = -1;
        instructionIndex = 0;
        chefAnimController = traineeChef.GetComponent<Animator>();
        int rand = Random.Range(0, 2);
        if (rand == 0)
        {
            allInstructions = smallInstructionList;
            isBigFire = false;
        }
        else
            allInstructions = largeInstructionList;

        if (traineeChef.GetComponent<RunAway>() != null)
            traineeChef.GetComponent<RunAway>().enabled = true;

#if UNITY_EDITOR
        isBigFire = false;
#endif

        smallFire.SetActive(!isBigFire);
        largeFire.SetActive(isBigFire);
    }

    // Update is called once per frame
    void Update ()
    {
        if(isScenarioDone)
        {
            Debug.Log("Scenario Completed");
            ScenarioHandler.instance.instruction.text = "Scenario Completed - bck btn to quit";
            //ScenarioHandler.instance.ScenarioQuit();
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
            ++step;
            Debug.Log(step);
        }

        switch(currState)
        {
            case STATE_SF.STATE_FIRE_START: // Buffer state
                chefAnimController.SetBool("running", true);
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
                {
                    fireBlanket.GetComponent<Animator>().SetBool("openFireBlanket", true);
                    smallFire.SetActive(false);
                    largeFire.SetActive(false);
                    isScenarioDone = true;
                }

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
            if (go.GetComponent<Outline>())
                go.GetComponent<Outline>().enabled = false;
        }

        ScenarioHandler.instance.interactableGO.Clear();
        foreach (GameObject go in sfInfoContainer[currState].interactables)
        {
            if (go.GetComponent<Outline>())
                go.GetComponent<Outline>().enabled = true;
            ScenarioHandler.instance.interactableGO.Add(go);

            Arrow.instance.objectToSnap = go;
        }
    }

    IEnumerator FireExtinguish()
    {
        yield return new WaitForSeconds(3f);

        smallFire.SetActive(false);
        largeFire.SetActive(false);

    }
}

