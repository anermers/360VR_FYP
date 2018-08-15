using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//reset all pickable obj back to their location

[System.Serializable]
public struct SCInfo
{
    public ScenarioCut.STATE_SC state;
    public List<GameObject> interactables;
    public List<string> instructions;
    public string description;
    public Sprite img;
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
        STATE_APPLY_YELLOW_ACRI,
        STATE_APPLY_BANDANGE,
        STATE_TOTAL
    };
    public GameObject traineeChef;
    public GameObject MedKit;
    public GameObject tempCollider;
    public GameObject MedTriggerLocal;
    public GameObject medKitCanvas;
    public GameObject progressBar;

    public List<SCInfo> scInfoList;
    public ParticleSystem gParticle;

    public AudioClip correctSound;
    public AudioClip incorrectSound;
    public AudioSource aSource;
    public AudioSource aInSource;

    public STATE_SC currState;
    STATE_SC prevState;
    //int instructionIndex;

    Dictionary<STATE_SC, SCInfo> scInfoContainer;

    private float timer;
    private Animator chefAnimController;
    private InstructionMenu instructionMenu;

    // Use this for initialization
    public void Start () {
        Debug.Log("sc start");
        traineeChef.transform.position = chefSpawnPoint.position;
        instructionMenu = ScenarioHandler.instance.instructionScreen;
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
                        go.GetComponent<cakeslice.Outline>() == null)
                    {
                        go.AddComponent<cakeslice.Outline>();
                        go.GetComponent<cakeslice.Outline>().color = 0;
                        go.GetComponent<cakeslice.Outline>().eraseRenderer = false;
                        go.GetComponent<cakeslice.Outline>().enabled = false;
                    }
                }
                // Adds to the dictionary
                scInfoContainer.Add(info.state, info);
            }
        }
        ScenarioHandler.instance.instructionScreen.PopulateInsutructionMenu();
        Init();
        SetInstruction();
    }

    public override void Init()
    {
        Debug.Log("ScenarioCut - Init");
        currState = STATE_SC.STATE_CUT_START;
        prevState = currState;
        isEventCompleted = false;
        isInteracted = false;
        isScenarioDone = false;
        //instructionIndex = 0;
        step = 0;
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
            ScenarioHandler.instance.description.text = "Scenario Completed - bck btn to quit";
            //ScenarioHandler.instance.ScenarioQuit();
        }

        if (prevState != currState)
        {
            Debug.Log("StateChanged");
            isEventCompleted = false;
            isInteracted = false;
            //reset index for instructions
            //instructionIndex = 0;
            ++step;
            SetCurrentInteractable();
            SetInstruction();
            prevState = currState;
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
                    progressBar.SetActive(true);
                    progressBar.GetComponent<ProgressBar>().currValue = 0f;
                    //SwitchState((int)STATE_SC.STATE_GET_MEDKIT);

                    if (AnimatorIsPlaying("After Cut Idle"))
                        SwitchState((int)STATE_SC.STATE_GET_MEDKIT);
                }
                break;
            case STATE_SC.STATE_GET_MEDKIT:
                //Player finds the medkit and brings in to a certain location
                isEventCompleted = isInteracted;
                Arrow.instance.objectToSnap = MedKit;
                if (isEventCompleted)
                {
                    EnableGreenEffect();
                    SwitchState((int)STATE_SC.STATE_GET_MEDKIT_TO_LOCAL);
                }
                break;
            case STATE_SC.STATE_GET_MEDKIT_TO_LOCAL:
                //Player brings the medkit to a certain location
                Arrow.instance.objectToSnap = MedTriggerLocal;
                MedTriggerLocal.SetActive(true);
                if (isEventCompleted)
                {
                    EnableGreenEffect();
                    Arrow.instance.objectToSnap = null;
                    tempCollider.SetActive(true);
                    MedTriggerLocal.SetActive(false);
                    medKitCanvas.SetActive(true);
                    aSource.PlayOneShot(correctSound);
                    SwitchState((int)STATE_SC.STATE_PURIFIED_WATER);
                }
                break;
            case STATE_SC.STATE_PURIFIED_WATER:
                //Get purified water and apply on traineeChef
                if (isEventCompleted)
                {
                    progressBar.GetComponent<ProgressBar>().AddProgress(35f);
                    aSource.PlayOneShot(correctSound);
                    SwitchState((int)STATE_SC.STATE_APPLY_GAUZE);
                }
                break;
            case STATE_SC.STATE_APPLY_GAUZE:
                //Get gauze and apply pressure on traineeChef
                if (isEventCompleted)
                {
                    aSource.PlayOneShot(correctSound);
                    SwitchState((int)STATE_SC.STATE_APPLY_YELLOW_ACRI);
                }     
                break;
            case STATE_SC.STATE_APPLY_YELLOW_ACRI:
                //Apply yellow ACRI solution
                if (isEventCompleted)
                {
                    aSource.PlayOneShot(correctSound);
                    progressBar.GetComponent<ProgressBar>().AddProgress(35f);
                    SwitchState((int)STATE_SC.STATE_APPLY_BANDANGE);
                }
                break;
            case STATE_SC.STATE_APPLY_BANDANGE:
                //Get bandage and apply on traineeChef
                if (isEventCompleted)
                {
                    if(!isScenarioDone)
                        aSource.PlayOneShot(correctSound);
                    progressBar.GetComponent<ProgressBar>().AddProgress(35f);
                    isScenarioDone = true;
                }
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
        if (ScenarioHandler.instance.description == null
             || scInfoContainer[currState].description == null)
            return;
        ScenarioHandler.instance.description.text = scInfoContainer[currState].description;
        instructionMenu.SwitchInstruction(step);
        ScenarioHandler.instance.displayImg.sprite = scInfoContainer[currState].img;
        if (HintHandler.instance != null)
            HintHandler.instance.SetHints(instructionMenu.currInstruction);
    }

    protected override void SetCurrentInteractable()
    {
        foreach (GameObject go in scInfoContainer[prevState].interactables)
        {
            if (go.GetComponent<cakeslice.Outline>())
                go.GetComponent<cakeslice.Outline>().enabled = false;
        }

        ScenarioHandler.instance.interactableGO.Clear();
        foreach (GameObject go in scInfoContainer[currState].interactables)
        {
            if (go.GetComponent<cakeslice.Outline>())
                go.GetComponent<cakeslice.Outline>().enabled = true;
            ScenarioHandler.instance.interactableGO.Add(go);
        }
    }
    //Functions to check if animations has ended or whihch animation is still playing
    bool AnimatorIsPlaying()
    {
        return chefAnimController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1;
    }

    bool AnimatorIsPlaying(string stateName)
    {
        return AnimatorIsPlaying() && chefAnimController.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }

    void EnableGreenEffect()
    {
        if (scInfoContainer[currState].interactables.Count <= 0)
            return;

        gParticle.transform.position = scInfoContainer[currState].interactables[0].transform.position;
        gParticle.GetComponent<ParticleController>().PlayParticle();
    }

    public void PlayIncorrectSound()
    {
        aSource.PlayOneShot(incorrectSound, 1);
    }
}
