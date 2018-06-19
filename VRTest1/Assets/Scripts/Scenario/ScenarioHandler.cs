using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioHandler : MonoBehaviour {

    public static ScenarioHandler instance = null;

    public List<ScenarioBase> scenarioList;
    public Text instruction;

    Dictionary<string, ScenarioBase> scenarioContainer;
    [HideInInspector]
    public List<GameObject> interactableGO;

    bool isScenarioActivated;
    //string currScenario;
    ScenarioBase currScenario;

    public Dictionary<string, ScenarioBase> ScenarioContainer { get { return scenarioContainer; } }
    //public string CurrScenario { get { return currScenario; } }
    public ScenarioBase CurrScenario { get { return currScenario; } }

    // Use this for initialization
    void Awake () {
        if (!instance)
            instance = this;

        currScenario = null;
        interactableGO = new List<GameObject>();
        isScenarioActivated = false;
        scenarioContainer = new Dictionary<string, ScenarioBase>();
        foreach(ScenarioBase sb in scenarioList)
        {
            // Init scenario
            sb.Init();

            // Deactivate scenarios on start up
            if (sb.gameObject.activeSelf)
                sb.gameObject.SetActive(false);
            // Add the scenario in
            if (!scenarioContainer.ContainsKey(sb.name))
                scenarioContainer.Add(sb.name, sb);
        }


        isScenarioActivated = true;
        currScenario = scenarioContainer["sf"];
        scenarioContainer["sf"].gameObject.SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.J))
        {

        }
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
        isScenarioActivated = true;
    }

    public void RandomScenarioType()
    {
        if (scenarioList.Count <= 0
            || isScenarioActivated)
            return;

        int index = Random.Range(0, scenarioList.Count - 1);
        scenarioList[index].gameObject.SetActive(true);
        currScenario = scenarioList[index];
        isScenarioActivated = true;
    }

    //public void OnSelected(Transform t)
    //{
    //    foreach(GameObject go in interactableGO)
    //    {
    //        if (t.gameObject.Equals(go))
    //        {
    //            Debug.Log("correct item selected");
    //            scenarioContainer[currScenario].IsInteracted = true;
    //            break;
    //        }
    //    }
    //}
}
