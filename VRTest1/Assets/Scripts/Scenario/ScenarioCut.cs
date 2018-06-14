using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SCInfo
{
    public ScenarioCut.STATE_SC state;
    public List<GameObject> interactables;
    public List<string> instructions;
}
public class ScenarioCut : ScenarioBase {
    public enum STATE_SC
    {
        STATE_CUT_START = 0,
        STATE_WASH_HANDS,
        STATE_GET_MEDKIT,
        STATE_PURIFIED_WATER,
        STATE_APPLY_GAUZE,
        STATE_APPLY_BANDANGE,
        STATE_TOTAL
    };
    public GameObject traineeChef;
    public Transform placeMedKitLocation;
    public Transform sinkTransform;

    public List<SCInfo> scInfoList;

    STATE_SC currState;
    STATE_SC prevState;
    int instructionIndex;

    Dictionary<STATE_SC, SCInfo> scInfoContainer;

    // Use this for initialization
    void Start () {
        //set the 1st state
        scInfoContainer = new Dictionary<STATE_SC, SCInfo>();
        foreach (SCInfo info in scInfoList)
        {
            if (!scInfoContainer.ContainsKey(info.state))
            {
                scInfoContainer.Add(info.state, info);
            }
        }
        currState = STATE_SC.STATE_CUT_START;
        prevState = currState;
        isEventCompleted = false;
        isInteracted = false;
        isScenarioDone = false;
        instructionIndex = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (prevState != currState)
        {
            Debug.Log("StateChanged");
            isEventCompleted = false;
            isInteracted = false;
            SetCurrentInteractable();
            //SetInstruction();
            prevState = currState;
            //reset index for instructions
            instructionIndex = 0;
        }

        switch(currState)
        {
            case STATE_SC.STATE_CUT_START:
                // Start scenario
                SwitchState((int)STATE_SC.STATE_WASH_HANDS);
                break;
            case STATE_SC.STATE_WASH_HANDS:
                // traineeChef goes to the sink and plays washing hands animation
                traineeChef.transform.LookAt(sinkTransform);

                if (Vector3.Distance(traineeChef.transform.position, sinkTransform.position) > 1)
                    traineeChef.transform.Translate(Vector3.forward * 1 * Time.deltaTime);
                else
                {
                    //rotate towards sink
                    //play wash hands animation
                    SwitchState((int)STATE_SC.STATE_GET_MEDKIT);
                }
                break;
            case STATE_SC.STATE_GET_MEDKIT:
                //Player finds the medkit and brings in to a certain location

                //Once medkit is at location, play animation on click
                    break;
            case STATE_SC.STATE_PURIFIED_WATER:
                //Get purified water and apply on traineeChef
                break;
            case STATE_SC.STATE_APPLY_GAUZE:
                //Get gauze and apply on traineeChef
                break;
            case STATE_SC.STATE_APPLY_BANDANGE:
                //Get bandage and apply on traineeChef
                break;
        }
    }


    protected override bool SwitchState(int index)
    {
        if (index < 0 || index >= (int)STATE_SC.STATE_TOTAL)
            return false;

        prevState = currState;
        currState = (STATE_SC)index;
        return true;
    }


    void SetInstruction()
    {
        if (ScenarioHandler.instance.instruction == null
            || scInfoContainer[currState].instructions.Count <= 0)
            return;
        ScenarioHandler.instance.instruction.text = scInfoContainer[currState].instructions[instructionIndex];
    }

    protected override void SetCurrentInteractable()
    {
        ScenarioHandler.instance.interactableGO.Clear();
        foreach (GameObject go in scInfoContainer[currState].interactables)
            ScenarioHandler.instance.interactableGO.Add(go);
    }

}
