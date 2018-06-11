using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonUIInteraction : MonoBehaviour {
    public UnityEngine.UI.Text outText;
    //public UnityEngine.UI.Text infoText;
    public GameObject playerCam;
    public GameObject playerController;
    public TextMesh infoText;
    protected Material oldHoverMat;
    public Material yellowMat;


    public void OnHoverEnter(Transform t) {

        //To get controller rotation (TO BE USED IN FUTURE)
        //Quaternion rot = OVRInput.GetLocalControllerRotation(activeController);

        //if (outText != null) {
        //    oldHoverMat = t.gameObject.GetComponent<Renderer>().material;
        //    t.gameObject.GetComponent<Renderer>().material = yellowMat;
        //    outText.text ="hovering over: " + t.gameObject.name;
        //}      
        oldHoverMat = t.gameObject.GetComponent<Renderer>().material;
        //t.gameObject.GetComponent<Renderer>().material = yellowMat;
        infoText.text = "";
        foreach (ItemInfo iter in RoomHandler.instance.RoomInfoContainer[RoomHandler.instance.CurrKey].itemList)
        {
            if (iter.item.Equals(t.gameObject))
            {
                infoText.text = iter.itemName;
                break;
            }
        }
        //Translate Text to point of interest
        infoText.transform.position = new Vector3(t.gameObject.transform.position.x, t.gameObject.transform.position.y + 2, t.gameObject.transform.position.z);
        //Rotate Text to face the camera
        infoText.transform.rotation = Quaternion.LookRotation(infoText.transform.position - playerCam.transform.position);
    }

    public void OnHoverExit(Transform t) {
        infoText.text = "";
        t.gameObject.GetComponent<Renderer>().material = oldHoverMat;
        //if (outText != null) {
        //    outText.text = "end over: " + t.gameObject.name;
        //}
    }

    public void OnSelected(Transform t) {
        //if (outText != null) {
        //    outText.text = "clicked on: " + t.gameObject.name;
        //}
      
        t.gameObject.GetComponent<Renderer>().material = yellowMat;
        //t.gameObject.transform.position = new Vector3(5, 5, 5);
        t.gameObject.transform.parent = playerController.transform;
        //t.parent = playerController.transform;
    }
}
