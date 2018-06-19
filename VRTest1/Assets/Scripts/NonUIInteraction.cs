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
    public Material redMat;
    public Material pinkMat;
    public GameObject snap;
    private bool isInteractable;

    public static bool objectSelected = false;

    public void OnHoverEnter(Transform t) {

        //To get controller rotation (TO BE USED IN FUTURE)
        //Quaternion rot = OVRInput.GetLocalControllerRotation(activeController);
        if (t.gameObject.GetComponent<Renderer>() != null)
        {
            oldHoverMat = t.gameObject.GetComponent<Renderer>().material;
            if (t.gameObject.GetComponent<cakeslice.Outline>() == null)
            {
                t.gameObject.AddComponent<cakeslice.Outline>();
                t.gameObject.GetComponent<cakeslice.Outline>().color = 1;
                t.gameObject.GetComponent<cakeslice.Outline>().eraseRenderer = false;
            }

            t.gameObject.GetComponent<cakeslice.Outline>().enabled = true;
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
        infoText.transform.position = new Vector3(t.gameObject.transform.position.x, t.gameObject.transform.position.y + 2, t.gameObject.transform.position.z);
        //Rotate Text to face the camera
        infoText.transform.rotation = Quaternion.LookRotation(infoText.transform.position - playerCam.transform.position);
        infoText.text = objectSelected.ToString();
    }

    public void OnHoverExit(Transform t) {

        if (t.gameObject.GetComponent<Renderer>() != null)
        {
            infoText.text = oldHoverMat.name;
            t.gameObject.GetComponent<Renderer>().material = oldHoverMat;
            t.gameObject.GetComponent<cakeslice.Outline>().enabled = false;
        }
    }

    public void OnSelected(Transform t) {
        Debug.Log("asdad");
        isInteractable = false;
        if (t.gameObject.tag == "PickUp")
        {
            if (!objectSelected)
            {
                objectSelected = true;
                if(t.gameObject.GetComponent<Renderer>()!=null)
                t.gameObject.GetComponent<Renderer>().material = yellowMat;

                //t.gameObject.transform.position = snap.transform.position;
                t.gameObject.transform.parent = playerController.transform;
                t.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            }
            else
            {
                objectSelected = false;
                t.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                t.gameObject.transform.parent = null;
            }
        }

        foreach (GameObject go in ScenarioHandler.instance.interactableGO)
        {
            if (t.gameObject.Equals(go))
            {
                Debug.Log("correct item selected");
                if (t.gameObject.GetComponent<Renderer>() != null)
                    t.gameObject.GetComponent<Renderer>().material = redMat;
                //send a message back
                ScenarioHandler.instance.CurrScenario.IsInteracted = true;
                ScenarioHandler.instance.CurrScenario.InteractedGO = go;
                isInteractable = true;
                break;
            }
        }

    }
}
