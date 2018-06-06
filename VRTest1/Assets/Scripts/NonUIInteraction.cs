using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonUIInteraction : MonoBehaviour {
    public UnityEngine.UI.Text outText;
    //public UnityEngine.UI.Text infoText;
    public GameObject playerCam;
    public TextMesh infoText;
    protected Material oldHoverMat;
    public Material yellowMat;


    public void OnHoverEnter(Transform t) {
        //if (outText != null) {
        //    oldHoverMat = t.gameObject.GetComponent<Renderer>().material;
        //    t.gameObject.GetComponent<Renderer>().material = yellowMat;
        //    outText.text ="hovering over: " + t.gameObject.name;
        //}      
        oldHoverMat = t.gameObject.GetComponent<Renderer>().material;
        t.gameObject.GetComponent<Renderer>().material = yellowMat;
        infoText.text = "INFO";
        foreach (ItemInfo iter in RoomHandler.instance.RoomInfoContainer[RoomHandler.instance.CurrKey].itemList)
        {
            if (iter.item.Equals(t.gameObject))
            {
                infoText.text = iter.itemName;
                break;
            }
        }

        infoText.transform.position = new Vector3(t.gameObject.transform.position.x, t.gameObject.transform.position.y + 2, t.gameObject.transform.position.z);
        infoText.transform.rotation = Quaternion.LookRotation(infoText.transform.position - playerCam.transform.position);
        //infoText.transform.LookAt(infoText.transform.position + playerCam.transform.rotation * Vector3.forward,
        //        playerCam.transform.rotation * Vector3.up);
        //infoText.transform.position = playerCam.WorldToViewportPoint(new Vector3(t.gameObject.transform.position.x, t.gameObject.transform.position.y, t.gameObject.transform.position.z));
    }

    public void OnHoverExit(Transform t) {
        infoText.text = "";
        t.gameObject.GetComponent<Renderer>().material = oldHoverMat;
        //if (outText != null) {
        //    outText.text = "end over: " + t.gameObject.name;
        //}
    }

    public void OnSelected(Transform t) {
        if (outText != null) {
            outText.text = "clicked on: " + t.gameObject.name;
        }
    }
}
