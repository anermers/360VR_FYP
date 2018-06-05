using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInteraction : MonoBehaviour {
    public UnityEngine.UI.Text outText;
    public GameObject kitchenModel;

	public void OnButtonClicked() {
        if (kitchenModel != null)
            if (EventSystem.current.currentSelectedGameObject.name == "VRKitchenButton")
                    kitchenModel.SetActive(!kitchenModel.activeInHierarchy);

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
