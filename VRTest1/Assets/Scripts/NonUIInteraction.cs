using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class NonUIInteraction : MonoBehaviour {
    public UnityEngine.UI.Text outText;
    //public UnityEngine.UI.Text infoText;
    public GameObject playerCam;
    public GameObject playerController;
    public TextMesh infoText;
    protected Material oldHoverMat;
    public Material yellowMat;
    public Material redMat;
    public Material pinkMat;
    public GameObject snap;

    public static bool objectSelected = false;

    public void OnHoverEnter(Transform t) {

        //To get controller rotation (TO BE USED IN FUTURE)
        //Quaternion rot = OVRInput.GetLocalControllerRotation(activeController);
        if (t.gameObject.GetComponent<Renderer>() != null)
        {
            oldHoverMat = t.gameObject.GetComponent<Renderer>().material;
            if (t.gameObject.GetComponent<Outline>() == null)
            {
                t.gameObject.AddComponent<Outline>();
                t.gameObject.GetComponent<Outline>().eraseRenderer = false;
            }

            t.gameObject.GetComponent<Outline>().color = 1;
            t.gameObject.GetComponent<Outline>().enabled = true;
            //t.gameObject.GetComponent<Renderer>().material = yellowMat;
        }       
        infoText.text = "";
        if(RoomHandler.instance.CurrKey != null)
        {
            foreach (ItemInfo iter in RoomHandler.instance.RoomInfoContainer[RoomHandler.instance.CurrKey].itemList)
            {
                if (iter.item.Equals(t.gameObject))
                {
                    infoText.text = iter.itemName;
                    break;
                }
            }
        }
        infoText.transform.position = new Vector3(t.gameObject.transform.position.x, t.gameObject.transform.position.y, t.gameObject.transform.position.z);
        infoText.transform.rotation = Quaternion.LookRotation(infoText.transform.position - playerCam.transform.position);
        //Rotate Text to face the camera
        if (t.gameObject.tag == "PickUp")
            infoText.text = t.gameObject.name;
    }

    public void OnHoverExit(Transform t) {

        if (t.gameObject.GetComponent<Renderer>() != null)
        {
            t.gameObject.GetComponent<Renderer>().material = oldHoverMat;
            t.gameObject.GetComponent<Outline>().enabled = false;
        }
        infoText.text = "";
    }

    public void OnSelected(Transform t) {

        foreach (GameObject go in ScenarioHandler.instance.interactableGO)
        {
            if (t.gameObject.Equals(go))
            {
                Debug.Log("correct item selected");
                if (t.gameObject.GetComponent<Renderer>() != null && t.gameObject.GetComponent<Outline>() != null)
                    t.gameObject.GetComponent<Outline>().color = 3;
                //send a message back
                ScenarioHandler.instance.CurrScenario.IsInteracted = true;
                ScenarioHandler.instance.CurrScenario.InteractedGO = go;
                break;
            }
        }
        if (t.gameObject.tag == "PickUp")
        {
            infoText.text = "";
            if (!objectSelected)
            {
                Debug.Log(t.gameObject.name);
                objectSelected = true;
                if(t.gameObject.GetComponent<Renderer>()!=null && t.gameObject.GetComponent<Outline>() != null)
                    t.gameObject.GetComponent<Outline>().color = 3;

                t.gameObject.transform.position = snap.transform.position;
                t.gameObject.transform.parent = playerController.transform;
                t.gameObject.GetComponent<Rigidbody>().isKinematic = true;
               
            }
            else
            {
              
                objectSelected = false;
                t.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                t.gameObject.transform.parent = null;

                if (t.gameObject.GetComponent<Renderer>() != null && t.gameObject.GetComponent<Outline>() != null)
                    t.gameObject.GetComponent<Outline>().color = 2;
                MedKitUI.Spawn = true;
            }
        }

    }
}
