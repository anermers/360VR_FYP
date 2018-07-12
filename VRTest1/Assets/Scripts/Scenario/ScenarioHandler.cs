using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioHandler : MonoBehaviour {

    public static ScenarioHandler instance = null;

    public List<ScenarioBase> scenarioList;
    public Text description;
    public Image displayImg;
    public InstructionMenu instructionScreen;
    public cakeslice.OutlineEffect olEffect;

    [HideInInspector]
    public List<GameObject> interactableGO;

    public GameObject playerCamera;
    public GameObject scenarioStartPos;

    Dictionary<string, ScenarioBase> scenarioContainer;
    bool isScenarioActivated;
    //string currScenario;
    ScenarioBase currScenario;

    public Dictionary<string, ScenarioBase> ScenarioContainer { get { return scenarioContainer; } }
    public ScenarioBase CurrScenario { get { return currScenario; } }

    // Use this for initialization
    void Awake () {
        if (!instance)
            instance = this;

        currScenario = null;
        interactableGO = new List<GameObject>();
        isScenarioActivated = false;
        scenarioContainer = new Dictionary<string, ScenarioBase>();
        olEffect.enabled = false;
        foreach (ScenarioBase sb in scenarioList)
        {
            // Deactivate scenarios on start up
            if (sb.gameObject.activeSelf)
                sb.gameObject.SetActive(false);
            // Add the scenario in
            if (!scenarioContainer.ContainsKey(sb.name))
                scenarioContainer.Add(sb.name, sb);
        }

        //testing    
        //currScenario = scenarioContainer["sc"];
        //scenarioContainer["sc"].gameObject.SetActive(true);
        //isScenarioActivated = true;
    }


    public void SelectScenarioType(string name)
    {
        if (!scenarioContainer.ContainsKey(name)
            || isScenarioActivated)
        {
            Debug.Log("scenario does not exist");
            return;
        }

        scenarioContainer[name].gameObject.SetActive(true);
        currScenario = scenarioContainer[name];
        //currScenario.Init();
        RoomHandler.instance.ShowMenu();

        //instructionScreen.PopulateInsutructionMenu();
        isScenarioActivated = true;
        PlayerToStartPos();
        olEffect.enabled = true;
    }

    public void RandomScenarioType()
    {
        if (scenarioList.Count <= 0
            || isScenarioActivated)
            return;


        RoomHandler.instance.ShowMenu();
        int index = Random.Range(0, scenarioList.Count - 1);
        scenarioList[index].gameObject.SetActive(true);
        currScenario = scenarioList[index];
        //currScenario.Init();
        //instructionScreen.PopulateInsutructionMenu();
        isScenarioActivated = true;
        PlayerToStartPos();
        olEffect.enabled = true;
    }

    public void ScenarioQuit()
    {
        description.text = "";
        currScenario.gameObject.SetActive(false);
        isScenarioActivated = false;
    }

    public void ScenarioFireSelect(bool bigFire)
    {
        scenarioContainer["sf"].gameObject.SetActive(true);
        currScenario = scenarioContainer["sf"];
        currScenario.GetComponent<ScenarioFire>().IsBigFire = bigFire;
        //currScenario.Init();
        RoomHandler.instance.ShowMenu();
        isScenarioActivated = true;
        PlayerToStartPos();
        olEffect.enabled = true;
    }

    private void PlayerToStartPos()
    {
        playerCamera.transform.position = scenarioStartPos.transform.position;
    }
}
