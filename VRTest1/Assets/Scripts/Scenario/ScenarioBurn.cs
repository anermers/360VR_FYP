using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

[System.Serializable]
public struct SBInfo
{
    public ScenarioBurn.STATE_SB state;
    public List<GameObject> interactables;
    public List<string> instructions;
}

public class ScenarioBurn : ScenarioBase
{
    public enum STATE_SB
    {
        STATE_BURN_START = 0,
        STATE_WASH_HANDS,
        STATE_GET_MEDKIT,
        STATE_GET_MEDKIT_TO_LOCAL,
        STATE_OPEN_MEDKIT,
        STATE_PURIFIED_WATER,
        STATE_APPLY_BANDANGE,
        STATE_TOTAL
    }

    public GameObject traineeChef;
    public GameObject MedKit;
    public GameObject tempCollider;
    public GameObject MedTriggerLocal;

    public List<SBInfo> sbInfoList;

    public STATE_SB currState;
    private STATE_SB prevState;
    private int instructionIndex;
    private Dictionary<STATE_SB, SBInfo> sbInfoContainer;

    private float timer;
    private Animator chefAnimController;

    // Use this for initialization
    private void Start () {
        currState = STATE_SB.STATE_BURN_START;
        prevState = currState;
        isEventCompleted = false;
        isInteracted = false;
        isScenarioDone = false;
        instructionIndex = 0;
        timer = 8.0f;
        chefAnimController = traineeChef.GetComponent<Animator>();
        MedTriggerLocal.SetActive(false);
    }

    public override void Init()
    {
        sbInfoContainer = new Dictionary<STATE_SB, SBInfo>();
        foreach (SBInfo info in sbInfoList)
        {
            if (!sbInfoContainer.ContainsKey(info.state))
            {
                sbInfoContainer.Add(info.state, info);
            }
        }
    }

    // Update is called once per frame
    private void Update () {
		if(prevState != currState)
        {
            Debug.Log("StateChanged: " + currState.ToString());
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
            case STATE_SB.STATE_BURN_START:
                // Start scenario
                timer -= 1 * Time.deltaTime;
                if (timer > 0)
                    chefAnimController.SetBool("beforeBurnIdle", true);
                else
                {
                    chefAnimController.SetBool("getsBurn", true);
                    chefAnimController.SetBool("afterBurnIdle", true);
                    SwitchState((int)STATE_SB.STATE_GET_MEDKIT);
                }
                break;
            case STATE_SB.STATE_WASH_HANDS:
                SwitchState((int)STATE_SB.STATE_GET_MEDKIT);
                break;
            case STATE_SB.STATE_GET_MEDKIT:
                //Player finds the medkit and brings in to a certain location
                isEventCompleted = isInteracted;
                if (isEventCompleted)
                    SwitchState((int)STATE_SB.STATE_GET_MEDKIT_TO_LOCAL);
                break;
            case STATE_SB.STATE_GET_MEDKIT_TO_LOCAL:
                //Player brings the medkit to a certain location
                MedTriggerLocal.SetActive(true);
                if (isEventCompleted)
                {
                    foreach (Transform child in MedKit.transform)
                    {
                        child.gameObject.AddComponent<Rigidbody>();
                        Debug.Log("RigidAdded");
                    }
                    tempCollider.SetActive(true);
                    MedTriggerLocal.SetActive(false);
                    SwitchState((int)STATE_SB.STATE_PURIFIED_WATER);
                }
                break;
            case STATE_SB.STATE_PURIFIED_WATER:
                //Get purified water and apply on traineeChef
                if (isEventCompleted)
                    SwitchState((int)STATE_SB.STATE_APPLY_BANDANGE);
                break;
            case STATE_SB.STATE_APPLY_BANDANGE:
                //Get bandage and apply on traineeChef
                if (isEventCompleted)
                    isScenarioDone = true;
                break;
        }

	}

    protected override bool SwitchState(int index)
    {
        if (index < 0 || index >= (int)STATE_SB.STATE_TOTAL)
            return false;

        prevState = currState;
        currState = (STATE_SB)index;
        return true;
    }


    void SetInstruction()
    {
        if (ScenarioHandler.instance.instruction == null
            || sbInfoContainer[currState].instructions.Count <= 0)
            return;
        ScenarioHandler.instance.instruction.text = sbInfoContainer[currState].instructions[instructionIndex];
    }

    protected override void SetCurrentInteractable()
    {
        foreach (GameObject go in sbInfoContainer[prevState].interactables)
        {
            if (go.GetComponent<Outline>())
                go.GetComponent<Outline>().enabled = false;
        }

        ScenarioHandler.instance.interactableGO.Clear();
        foreach (GameObject go in sbInfoContainer[currState].interactables)
        {
            if (go.GetComponent<Outline>())
                go.GetComponent<Outline>().enabled = true;
            ScenarioHandler.instance.interactableGO.Add(go);
        }
    }
}
