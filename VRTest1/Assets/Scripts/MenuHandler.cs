using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour {

    public GameObject fireBtn;
    public GameObject burnObj;
    public GameObject cutObj;
    public Text debugTxt;

	// Use this for initialization
	void Start () {
		
	}

    public void OnClickSelectScenario(string key)
    {
        ScenarioHandler.instance.SelectScenarioType(key);
    }

    public void OnClickRandomScenario()
    {
        ScenarioHandler.instance.RandomScenarioType();
    }

    public void OnPointerEnter(Transform t)
    {
        Debug.Log("POINTER ENTERED " + t.name);
        t.localScale = new Vector3(0.8f, 0.8f, 1);
    }

    public void OnPointerExit(Transform t)
    {
        Debug.Log("POINTER EXITED " + t.name);
        t.localScale = new Vector3(1, 1, 1);
    }
}
