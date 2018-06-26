using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInteraction : MonoBehaviour {
    public UnityEngine.UI.Text outText;
    public GameObject kitchenModel;
    public GameObject menuUI;
    public GameObject selectionRoom;

    public void OnButtonClicked() {
        if (kitchenModel != null)
            //Check the button name and set kitchen model to active
            if (EventSystem.current.currentSelectedGameObject.name == "VRKitchenButton")
            {
                menuUI.SetActive(false);
                selectionRoom.SetActive(false);
                kitchenModel.SetActive(!kitchenModel.activeInHierarchy);
            }
            else
                kitchenModel.SetActive(false);

        else if (outText != null) {
            outText.text = "UI Button clicked";
        }
    }

    public void OnSliderChanged(float value) {
        if (outText != null) {
            outText.text = "UI Slider value: " + value;
        }
    }

}
