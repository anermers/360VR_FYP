using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

//reset all pickable obj back to their location

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
        STATE_GET_MEDKIT_TO_LOCAL,
        STATE_OPEN_MEDKIT,
        STATE_PURIFIED_WATER,
        STATE_APPLY_GAUZE,
        STATE_APPLY_BANDANGE,
        STATE_TOTAL
    };
    public GameObject traineeChef;
    public GameObject MedKit;
    public GameObject tempCollider;
    public GameObject MedTriggerLocal;
    public GameObject medKitCanvas;

    public List<SCInfo> scInfoList;

    public STATE_SC currState;
    STATE_SC prevState;
    int instructionIndex;

    Dictionary<STATE_SC, SCInfo> scInfoContainer;

    private float timer;
    private Animator chefAnimController;

    // Use this for initialization
    public void Start () {
        Debug.Log("sc start");
        traineeChef.transform.position = chefSpawnPoint.position;
        allInstructions = new List<string>();
        scInfoContainer = new Dictionary<STATE_SC, SCInfo>();
        chefAnimController = traineeChef.GetComponent<Animator>();
        chefAnimController.SetBool("placeDishIdle", false);
        foreach (SCInfo info in scInfoList)
        {
            // adds the instructions to allinstruction list
            if (info.instructions.Count > 0)
                allInstructions.Add(info.instructions[0]);

            if (!scInfoContainer.ContainsKey(info.state))
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
                scInfoContainer.Add(info.state, info);
            }
        }
        ScenarioHandler.instance.instructionScreen.PopulateInsutructionMenu();
        Init();
    }

    public override void Init()
    {
        Debug.Log("ScenarioCut - Init");
        currState = STATE_SC.STATE_CUT_START;
        prevState = currState;
        isEventCompleted = false;
        isInteracted = false;
        isScenarioDone = false;
        instructionIndex = 0;
        step = -1;
        timer = 5.0f;
        MedTriggerLocal.SetActive(false);
        medKitCanvas.SetActive(false);

        if (traineeChef.GetComponent<GetCut>() != null)
            traineeChef.GetComponent<GetCut>().enabled = true;
    }

    // Update is called once per frame
    void Update () {

        if (isScenarioDone)
        {
            Debug.Log("Scenario Completed");
            ScenarioHandler.instance.instruction.text = "Scenario Completed - bck btn to quit";
            //ScenarioHandler.instance.ScenarioQuit();
        }

        if (prevState != currState)
        {
            Debug.Log("StateChanged: " + currState.ToString());
            isEventCompleted = false;
            isInteracted = false;
            SetCurrentInteractable();
            SetInstruction();
            prevState = currState;
            //reset index for instructions
            instructionIndex = 0;
            ++step;
            //Debug.Log(step);
        }

        switch (currState)
        {
            case STATE_SC.STATE_CUT_START:
                // Start scenario
                timer -= 1 * Time.deltaTime;
                if (timer > 0)
                    chefAnimController.SetBool("beforeCutIdle", true);
                else
                {
                    chefAnimController.SetBool("getsCut", true);
                    chefAnimController.SetBool("afterCutIdle", true);
                    SwitchState((int)STATE_SC.STATE_GET_MEDKIT);
                }
                break;
            case STATE_SC.STATE_GET_MEDKIT:
                //Player finds the medkit and brings in to a certain location
                isEventCompleted = isInteracted;
                Arrow.instance.objectToSnap = MedKit;
                if (isEventCompleted)
                    SwitchState((int)STATE_SC.STATE_GET_MEDKIT_TO_LOCAL);
                break;
            case STATE_SC.STATE_GET_MEDKIT_TO_LOCAL:
                //Player brings the medkit to a certain location
                Arrow.instance.objectToSnap = MedTriggerLocal;
                MedTriggerLocal.SetActive(true);
                if (isEventCompleted)
                {
                    Arrow.instance.objectToSnap = null;
                    tempCollider.SetActive(true);
                    MedTriggerLocal.SetActive(false);
                    medKitCanvas.SetActive(true);
                    SwitchState((int)STATE_SC.STATE_PURIFIED_WATER);
                }
                break;
            case STATE_SC.STATE_PURIFIED_WATER:
                //Get purified water and apply on traineeChef
                if (isEventCompleted)
                    SwitchState((int)STATE_SC.STATE_APPLY_GAUZE);
                break;
            case STATE_SC.STATE_APPLY_GAUZE:
                //Get gauze and apply on traineeChef
                if (isEventCompleted)
                    SwitchState((int)STATE_SC.STATE_APPLY_BANDANGE);
                break;
            case STATE_SC.STATE_APPLY_BANDANGE:
                //Get bandage and apply on traineeChef
                if (isEventCompleted)
                    isScenarioDone = true;
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
        foreach (GameObject go in scInfoContainer[prevState].interactables)
        {
            if (go.GetComponent<Outline>())
                go.GetComponent<Outline>().enabled = false;
        }

        ScenarioHandler.instance.interactableGO.Clear();
        foreach (GameObject go in scInfoContainer[currState].interactables)
        {
            if (go.GetComponent<Outline>())
                go.GetComponent<Outline>().enabled = true;
            ScenarioHandler.instance.interactableGO.Add(go);
        }
    }

}
