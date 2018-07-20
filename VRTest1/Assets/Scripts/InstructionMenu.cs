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
                gameObject.transform.position = uiCamera.transform.forward * 1.5f + uiCamera.transform.position;
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, 1.5f, gameObject.transform.position.z);
                gameObject.transform.rotation = Quaternion.LookRotation((gameObject.transform.position - player.transform.position).normalized);
            }

            //gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, 0);
            // gameObject.transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            //// transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
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

            // Setting alpha of the img
            Image image = baseText.GetComponent<Image>();
            image.enabled = false;
            //var tempColor = image.color;
            //tempColor.a = 0f;
            //image.color = tempColor;

            baseText.transform.SetParent(layoutPanel.transform, false);
            EventTrigger trigger = baseText.transform.GetChild(0).GetComponent<EventTrigger>();
            trigger.triggers[0].callback.AddListener(delegate { OnPointerEnter(baseText.transform); });
            trigger.triggers[1].callback.AddListener(delegate { OnPointerExit(baseText.transform); });
            trigger.triggers[2].callback.AddListener(delegate { OnPointerClick(baseText.transform); });
            baseText.transform.GetChild(0).GetComponent<Text>().text = instruction;
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
        //Text textObj = t.gameObject.GetComponent<Text>();
        //Debug.Log(t.name + " CLICKED");
        //if (textObj != null)
        //{
        //    if(isSelected)
        //    {
        //        prevSelectedObj.GetComponent<Text>().fontStyle = FontStyle.Normal;
        //        prevSelectedObj.GetComponent<Text>().color = Color.white;
        //        isSelected = false;
        //    }

        //    textObj.fontStyle = FontStyle.Bold;
        //    textObj.color = Color.yellow;
        //    isSelected = true;
        //    prevSelectedObj = t;
        //}
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

    public string StrikeThrough(string s)
    {
        string strikethrough = "";
        foreach (char c in s)
        {
            strikethrough = strikethrough + c + '\u0336';
        }
        return strikethrough;
    }

    public void SwitchInstruction(int index)
    {
        if (index < 0)
            return;

        if (!ScenarioHandler.instance.CurrScenario.IsScenarioDone)
        {
            Debug.Log(index);
            //layoutPanel.transform.GetChild(index).GetComponent<Image>().color = new Color(Color.white.r, Color.white.g, Color.white.b, 1f);
            layoutPanel.transform.GetChild(index).GetComponent<Image>().enabled = true;
            layoutPanel.transform.GetChild(index).transform.GetChild(0).transform.GetComponent<Text>().color = Color.cyan;
            if(index > 0)
            {
                //layoutPanel.transform.GetChild(index - 1).GetComponent<Image>().color = new Color(Color.white.r, Color.white.g, Color.white.b, 0f);
                layoutPanel.transform.GetChild(index - 1).GetComponent<Image>().enabled = false;
                layoutPanel.transform.GetChild(index - 1).transform.GetChild(0).GetComponent<Text>().text = StrikeThrough(layoutPanel.transform.GetChild(index - 1).transform.GetChild(0).GetComponent<Text>().text);
            }
        

        }
    }

    public void CloseInstructionMenu()
    {
        isMenuEnabled = false;
        gameObject.SetActive(false);
    }
}
