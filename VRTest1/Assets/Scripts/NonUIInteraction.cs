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
    private GameObject temp;

    public static bool objectSelected = false;


    public void OnHoverEnter(Transform t) {

        //To get controller rotation (TO BE USED IN FUTURE)
        //Quaternion rot = OVRInput.GetLocalControllerRotation(activeController);

    

        if (t.gameObject.GetComponent<Renderer>() != null)
        {
            oldHoverMat = t.gameObject.GetComponent<Renderer>().material;
            t.gameObject.GetComponent<Renderer>().material = yellowMat;
        }       
        oldHoverMat = t.gameObject.GetComponent<Renderer>().material;
        t.gameObject.GetComponent<Renderer>().material = yellowMat;
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
        infoText.text = oldHoverMat.name;
        t.gameObject.GetComponent<Renderer>().material = oldHoverMat;
    }

    public void OnSelected(Transform t) {
    if(t.gameObject.tag == "PickUp")
        {                     
            if(!objectSelected)
            {
                    objectSelected = true;
                    t.gameObject.GetComponent<Renderer>().material = yellowMat;       
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

        if (t.gameObject.GetComponent<Animator>() != null)
            t.gameObject.GetComponent<Animator>().SetBool("Play", true);

    }
}
