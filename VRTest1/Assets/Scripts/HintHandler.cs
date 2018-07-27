using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintHandler : MonoBehaviour {

    public static HintHandler instance = null;

    public Text hintText;
    public Text headerText;
    public Image hintImage;

    public GameObject confirmationPanel;
    public GameObject hintPanel;

    private void Awake()
    {
        if (!instance)
            instance = this;

        hintPanel.SetActive(false);
        confirmationPanel.SetActive(true);
        //gameObject.SetActive(false);
    }

    public void DisplayHint(bool bHint)
    {
        hintPanel.SetActive(bHint);
        confirmationPanel.SetActive(!bHint);
        //gameObject.SetActive(bHint);
    }

    public void SetHints(string strHint)
    {
        hintText.text = strHint;
        hintImage.sprite = ScenarioHandler.instance.displayImg.sprite;
        headerText.text = ScenarioHandler.instance.Header.text;
    }
}
