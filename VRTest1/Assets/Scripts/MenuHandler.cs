using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuHandler : MonoBehaviour {

    public Text debugTxt;
    public GameObject instructionScreen;
    public GameObject menuScreen;
    public GameObject tutorialScreen;
    public RayPointer pointer;
    
	void Update () {

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!tutorialScreen.activeSelf)
                tutorialScreen.GetComponent<TutorialHandler>().DisplayTutorial();
            else
                tutorialScreen.GetComponent<TutorialHandler>().CloseTutorial();
        }
#endif
        //if (!menuScreen.activeSelf && !tutorialScreen.activeSelf)
        //    pointer.rayLength = 2;



        //bool noUIcontrolsInUse = EventSystem.current.currentSelectedGameObject == null; EventSystems.EventSystem.IsPointerOverGameObject

        //if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1))
        //    Debug.Log(EventSystem.current.currentSelectedGameObject.name);

        if (ScenarioHandler.instance.CurrScenario == null)
            return;

        if(ScenarioHandler.instance.CurrScenario.IsScenarioDone)
        {
            instructionScreen.SetActive(true);
            instructionScreen.GetComponent<InstructionMenu>().DispayWinScreen();
        }


    }

    public void OnClickSelectScenario(string key)
    {
        if (!MainMenuCamera.isMoving)
            ScenarioHandler.instance.SelectScenarioType(key);
    }

    public void OnClickFireScenario(bool bigFire)
    {
        if (!MainMenuCamera.isMoving)
            ScenarioHandler.instance.ScenarioFireSelect(bigFire);
    }

    public void OnClickRandomScenario()
    {
        if (!MainMenuCamera.isMoving)
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
