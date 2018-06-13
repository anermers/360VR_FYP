using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioHandler : MonoBehaviour {

    public List<ScenarioBase> scenarioList;
    public Text instruction;

    Dictionary<string,ScenarioBase> scenarioContainer;

    bool isScenarioActivated;

	// Use this for initialization
	void Awake () {
        isScenarioActivated = false;
        scenarioContainer = new Dictionary<string, ScenarioBase>();
        foreach(ScenarioBase sb in scenarioList)
        {
            // Deactivate scenarios on start up
            if (sb.gameObject.activeSelf)
                sb.gameObject.SetActive(false);
            // Add the scenario in
            if (!scenarioContainer.ContainsKey(sb.name))
                scenarioContainer.Add(sb.name, sb);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
        

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
        isScenarioActivated = true;
    }

    public void RandomScenarioType()
    {
        if (scenarioList.Count <= 0
            || isScenarioActivated)
            return;

        int index = Random.Range(0, scenarioList.Count - 1);
        scenarioList[index].gameObject.SetActive(true);
        isScenarioActivated = true;
    }
}
