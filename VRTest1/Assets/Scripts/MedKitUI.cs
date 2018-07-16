using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public class ObjectInfo
{
    public GameObject Object;
    public string description;
}

public class MedKitUI : MonoBehaviour {
    public List<ObjectInfo> inventory;
    
    public GameObject previewPosition;
    public GameObject temp;
    public Text objectName;
    public Text descriptionText;
    public static bool Spawn = false;
    private int selection;
    private Vector3 originalScale;


    private List<GameObject> spawnedObjects;
    // Use this for initialization
    void Start () {
        selection = 0;
        Spawn = false;
        spawnedObjects = new List<GameObject>();
        originalScale = new Vector3(0, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.M))
        {
            SetObject(5);
        }
        if (Input.GetKey(KeyCode.N))
        {
            SetObject(2);
        }       

        if(Spawn)
            SpawnObject();   
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
    //On click functions
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
    public void OnPointerEnter(Transform t)
    {
        //Debug.Log("POINTER ENTERED " + t.name);
        originalScale = t.transform.localScale;
        t.localScale = new Vector3(1.2f, 1.2f, 1);
    }

    public void OnPointerExit(Transform t)
    {
        //Debug.Log("POINTER EXITED " + t.name);
        t.localScale = originalScale;
    }
}
