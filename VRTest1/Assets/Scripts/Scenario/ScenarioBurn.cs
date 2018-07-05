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
    public string description;
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
    public GameObject medKitCanvas;
    public List<SBInfo> sbInfoList;

    public STATE_SB currState;
    private STATE_SB prevState;
    private int instructionIndex;
    private Dictionary<STATE_SB, SBInfo> sbInfoContainer;

    private float timer;
    private Animator chefAnimController;

    // Use this for initialization
    public void Start () {

        traineeChef.transform.position = chefSpawnPoint.position;
        sbInfoContainer = new Dictionary<STATE_SB, SBInfo>();
        allInstructions = new List<string>();
        chefAnimController = traineeChef.GetComponent<Animator>();
        // Set curr playing animation to false to transition to the next animation
        chefAnimController.SetBool("placeDishIdle", false);

        // Populating of dict.
        foreach (SBInfo info in sbInfoList)
        {
            // adds the instructions to allinstruction list
            if (info.instructions.Count > 0)
                allInstructions.Add(info.instructions[0]);

            if (!sbInfoContainer.ContainsKey(info.state))
            {
                // Adds outline component to each GO in the scenario
                foreach (GameObject go in info.interactables)
                {
                    if (go == null)
                        continue;

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
                sbInfoContainer.Add(info.state, info);
            }
        }
        ScenarioHandler.instance.instructionScreen.PopulateInsutructionMenu();
        Init();
    }

    public override void Init()
    {
        Debug.Log("ScenarioBurn - Init");
        //set the curr state
        currState = STATE_SB.STATE_BURN_START;
        prevState = currState;
        //setting of booleans
        isEventCompleted = false;
        isInteracted = false;
        isScenarioDone = false;
        //setting of other variables
        instructionIndex = 0;
        // starting index of curr instruction to follow
        step = 0;
        timer = 8.0f;
        // settign of active for gameObject
        MedTriggerLocal.SetActive(false);
        medKitCanvas.SetActive(false);

        // Enabling of related scripts
        if (traineeChef.GetComponent<GetBurn>() != null)
            traineeChef.GetComponent<GetBurn>().enabled = true;
    }

    // Update is called once per frame
    private void Update () {

        if (isScenarioDone)
        {
            Debug.Log("Scenario Completed");
            ScenarioHandler.instance.description.text = "Scenario Completed - bck btn to quit";
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
                Arrow.instance.objectToSnap = MedKit;
                if (isEventCompleted)
                    SwitchState((int)STATE_SB.STATE_GET_MEDKIT_TO_LOCAL);
                break;
            case STATE_SB.STATE_GET_MEDKIT_TO_LOCAL:
                //Player brings the medkit to a certain location
                Arrow.instance.objectToSnap = MedTriggerLocal;
                MedTriggerLocal.SetActive(true);
                if (isEventCompleted)
                {
                    medKitCanvas.SetActive(true);
                    MedTriggerLocal.SetActive(true);
                    Arrow.instance.objectToSnap = null;
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
        if (ScenarioHandler.instance.description == null
              || sbInfoContainer[currState].description == null)
            return;

        ScenarioHandler.instance.description.text = sbInfoContainer[currState].description;
    }

    protected override void SetCurrentInteractable()
    {
        foreach (GameObject go in sbInfoContainer[prevState].interactables)
        {
            if (go == null)
                continue;

            if (go.GetComponent<Outline>())
                go.GetComponent<Outline>().enabled = false;
        }

        ScenarioHandler.instance.interactableGO.Clear();
        foreach (GameObject go in sbInfoContainer[currState].interactables)
        {
            if (go == null)
                continue;

            if (go.GetComponent<Outline>())
                go.GetComponent<Outline>().enabled = true;
            ScenarioHandler.instance.interactableGO.Add(go);
        }
    }
}
