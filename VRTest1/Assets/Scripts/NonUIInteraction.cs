using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonUIInteraction : MonoBehaviour {
    public UnityEngine.UI.Text outText;

    protected Material oldHoverMat;
    public Material yellowMat;

    public void OnHoverEnter(Transform t) {
        if (outText != null) {
            oldHoverMat = t.gameObject.GetComponent<Renderer>().material;
            t.gameObject.GetComponent<Renderer>().material = yellowMat;
            outText.text ="hovering over: " + t.gameObject.name;
        }
    }

    public void OnHoverExit(Transform t) {
        if (outText != null) {
            t.gameObject.GetComponent<Renderer>().material = oldHoverMat;
            outText.text = "end over: " + t.gameObject.name;
        }
    }

    public void OnSelected(Transform t) {
        if (outText != null) {
            outText.text = "clicked on: " + t.gameObject.name;
        }
    }
}
