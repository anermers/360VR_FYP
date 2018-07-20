using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct TutorialInfo
{
    public string key;
    public string description;
    public Sprite img;
    public TutorialHandler.TUTORIAL_STAGE stage;
}

public class TutorialHandler : MonoBehaviour {

    public enum TUTORIAL_STAGE
    {
        STAGE_NAVIGATION = 0,
        STAGE_INTERACTION,
        STAGE_OBJECTIVE,
        STAGE_FIRSTAID_EXAMPLE,
        STAGE_COMPLETED,
        STAGE_TOTAL
    }

    public GameObject faExampleScreen;
    public GameObject completedScreen;
    public GameObject defaultScreen;

    public Text header;
    public Text body;
    public Image icon;

    public List<TutorialInfo> infoList;

    public bool isTutorial = false;
    public TUTORIAL_STAGE currStage;

    private Dictionary<TUTORIAL_STAGE, TutorialInfo> tutorialContainer;

    // Use this for initialization
    void Awake()
    {
        tutorialContainer = new Dictionary<TUTORIAL_STAGE, TutorialInfo>();
        foreach (TutorialInfo info in infoList)
        {
            if (!tutorialContainer.ContainsKey(info.stage))
                tutorialContainer.Add(info.stage, info);
        }
        currStage = 0;
        SetTutorialInfo();
    }

    // Update is called once per frame
    void Update() {

        if (!isTutorial)
            gameObject.SetActive(false);

        //Vector2 primaryAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

        if (OVRInput.Get(OVRInput.Button.DpadLeft)
            || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            --currStage;
            SetTutorialInfo();
        }
        else if (OVRInput.Get(OVRInput.Button.DpadRight)
            || Input.GetKeyDown(KeyCode.RightArrow))
        {
            ++currStage;
            SetTutorialInfo();
        }




        switch (currStage)
        {
            case TUTORIAL_STAGE.STAGE_FIRSTAID_EXAMPLE:
                completedScreen.SetActive(false);
                defaultScreen.SetActive(false);
                faExampleScreen.SetActive(true);
                break;
            case TUTORIAL_STAGE.STAGE_COMPLETED:
                completedScreen.SetActive(true);
                defaultScreen.SetActive(false);
                faExampleScreen.SetActive(false);
                break;
        }
    }

    public void CloseTutorial()
    {
        isTutorial = false;
    }

    public void DisplayTutorial()
    {
        isTutorial = true;
        gameObject.SetActive(isTutorial);
        currStage = 0;
    }

    private void SetTutorialInfo()
    {
        if (currStage >= TUTORIAL_STAGE.STAGE_TOTAL)
            currStage = TUTORIAL_STAGE.STAGE_COMPLETED;
        else if (currStage < 0)
            currStage = TUTORIAL_STAGE.STAGE_NAVIGATION;

        Debug.Log(currStage.ToString());
        header.text = tutorialContainer[currStage].key;
        body.text = tutorialContainer[currStage].description;
        icon.sprite = tutorialContainer[currStage].img;
        defaultScreen.SetActive(true);
        completedScreen.SetActive(false);
        faExampleScreen.SetActive(false);
    }
}
