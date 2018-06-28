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
    public static bool Spawn = false;
    private int selection;


    private List<GameObject> spawnedObjects;
    // Use this for initialization
    void Start () {
        selection = 50;
        Spawn = false;
        spawnedObjects = new List<GameObject>();
        for (int i = 0; i < inventory.Count; ++i)
        {
            if(inventory[i].Object !=null)
                inventory[i].originalTransform = inventory[i].Object.transform;    
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.M))
        {
            SetObject(1);
        }
        if (Input.GetKey(KeyCode.N))
        {
            SetObject(2);
        }       

        if(Spawn)
            SpawnObject();   
    }
    //On click function
    public void SetObject(int _selection)
    {
        if (selection != _selection)
        {
            foreach (GameObject iter in spawnedObjects)
                Destroy(iter);

            selection = _selection;
            SpawnObject();
            SetName();
            SetDescription();
        }
    }
    void SetName()
    {
        objectName.text = inventory[selection].Object.name;
    }
    void SetDescription()
    {
        descriptionText.text = inventory[selection].description;
    }

    //Spawns an instatiated version of the object
    void SpawnObject()
    {
        NonUIInteraction.objectSelected = false;
        temp = Instantiate(inventory[selection].Object, previewPosition.transform.position, Quaternion.identity);
        temp.tag = "PickUp";
        temp.AddComponent<BoxCollider>();
        temp.AddComponent<Rigidbody>();
        temp.GetComponent<Rigidbody>().isKinematic = true;
        Spawn = false;
        spawnedObjects.Add(temp);
    }
}
