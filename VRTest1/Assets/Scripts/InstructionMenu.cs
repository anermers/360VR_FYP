using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InstructionMenu : MonoBehaviour {


    [Header("Populate layout")]
    public GameObject textPrefab;
    public GameObject layoutPanel;
    public GameObject imgPanel;
    public Image previewImg;
    [Header("Other Misc.")]
    public GameObject uiCamera;
    public GameObject player;
    public GameObject completedScreen;
    public List<Texture> imgList;

    public bool isMenuEnabled = false;

    private bool isSelected = false;
    private Transform prevSelectedObj = null;

    // Update is called once per frame
    void Update () {

        if(isMenuEnabled)
        {
            // check if instruction is within the camera view
            Vector3 screenPoint = uiCamera.GetComponent<Camera>().WorldToViewportPoint(transform.position);
            bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.z < 2;

            //if not in view 
            if (!onScreen)
            {
                // user can move nad menu is disable
                gameObject.transform.position = uiCamera.transform.forward * 2f + uiCamera.transform.position;
                gameObject.transform.rotation = Quaternion.LookRotation(gameObject.transform.position - player.transform.position);
            }

            if(!ScenarioHandler.instance.CurrScenario.IsScenarioDone)
            {
                for (int i = 0; i <= ScenarioHandler.instance.CurrScenario.step; ++i)
                    layoutPanel.transform.GetChild(i).GetComponent<Text>().color = Color.green;
            }   
        }
    }

    public void DisplayInstructionMenu()
    {
        if (player == null)
            Debug.Log("NULL");
        player.GetComponent<OVRPlayerController>().GravityModifier = 0.7f;
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void PopulateInsutructionMenu()
    {
        // Should be called once in Init of the scenario
        List<string> instructions = ScenarioHandler.instance.CurrScenario.AllInstructions;
        foreach (string instruction in instructions)
        {
            GameObject baseText = Instantiate(textPrefab);
            baseText.transform.SetParent(layoutPanel.transform, false);
            EventTrigger trigger = baseText.GetComponent<EventTrigger>();
            trigger.triggers[0].callback.AddListener(delegate { OnPointerEnter(baseText.transform); });
            trigger.triggers[1].callback.AddListener(delegate { OnPointerExit(baseText.transform); });
            trigger.triggers[2].callback.AddListener(delegate { OnPointerClick(baseText.transform); });
            baseText.GetComponent<Text>().text = instruction;
        }
    }

    public void OnPointerEnter(Transform t)
    {
        Debug.Log(t.name + " Enter");
        if(t.gameObject.GetComponent<Text>() != null)
        {
            t.gameObject.GetComponent<Text>().fontStyle = FontStyle.Bold;
        }
    }

    public void OnPointerExit(Transform t)
    {
        Debug.Log(t.name + " Exit");
        if (t.gameObject.GetComponent<Text>() != null)
        {
            t.gameObject.GetComponent<Text>().fontStyle = FontStyle.Normal;
        }
    }

    public void OnPointerClick(Transform t)
    {
        Text textObj = t.gameObject.GetComponent<Text>();
        Debug.Log(t.name + " CLICKED");
        if (textObj != null)
        {
            if(isSelected)
            {
                prevSelectedObj.GetComponent<Text>().fontStyle = FontStyle.Normal;
                prevSelectedObj.GetComponent<Text>().color = Color.white;
                isSelected = false;
            }

            textObj.fontStyle = FontStyle.Bold;
            textObj.color = Color.yellow;
            isSelected = true;
            prevSelectedObj = t;
        }
    }

    public void DispayWinScreen()
    {
        if (ScenarioHandler.instance.CurrScenario.IsScenarioDone)
        {
            isMenuEnabled = true;
            gameObject.SetActive(true);
            layoutPanel.SetActive(false);
            completedScreen.SetActive(true);
            imgPanel.SetActive(false);
        }
    }

    public void FillImage(int index)
    {
    }
}
