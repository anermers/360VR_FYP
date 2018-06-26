using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ObjectInfo
{
    public GameObject Object;
    public string description;

    public Transform originalTransform;
}

public class MedKitUI : MonoBehaviour {
    public List<ObjectInfo> inventory;
    
    public GameObject previewPosition;
    public GameObject temp;
    public Text objectName;
    public Text descriptionText;


    private int selection;
    // Use this for initialization
    void Start () {
        selection = 0;
        //inventory = new List<ObjectInfo>();
        for (int i = 0; i < inventory.Count; ++i)
        {
            if(inventory[i].Object !=null)
                inventory[i].originalTransform = inventory[i].Object.transform;    
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.M))
        {
            inventory[selection].Object.GetComponent<Rigidbody>().isKinematic = true;
            SetToOriginalTransform();
            SnapToPreview();
            SetName();
            SetDescription();
        }
        if (Input.GetKey(KeyCode.N))
        {
            Destroy(temp);
            temp = null;
        }
    }

    //On click function
    public void SetObject(int _selection)
    {
        if (selection != _selection)
        {
            if(temp!=null)
            {
                Destroy(temp);
                temp = new GameObject();
            }
            SetToOriginalTransform();
            selection = _selection;
            inventory[selection].Object.GetComponent<Rigidbody>().isKinematic = true;
            SnapToPreview();
            SetName();
            SetDescription();
        }
    }

    void SnapToPreview()
    {
        temp = Instantiate(inventory[selection].Object, previewPosition.transform.position, Quaternion.identity);
        //inventory[selection].Object.transform.position = previewPosition.transform.position;
    }

    void SetToOriginalTransform()
    {
        inventory[selection].Object.transform.position = inventory[selection].originalTransform.position;
    }
    void SetName()
    {
        objectName.text = inventory[selection].Object.name;
    }
    void SetDescription()
    {
        descriptionText.text = inventory[selection].description;
    }
}
