using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInteraction : MonoBehaviour {
    public UnityEngine.UI.Text outText;

	public void OnButtonClicked() {
        if (outText != null) {
            outText.text = "UI Button clicked";
        }
    }

    public void OnSliderChanged(float value) {
        if (outText != null) {
            outText.text = "UI Slider value: " + value;
        }
    }
}
