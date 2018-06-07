using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ItemInfo
{
    public string itemName;
    public GameObject item;
}

[System.Serializable]
public class RoomInfo
{
    public string name;
    public Texture roomTexture;
    public List<ItemInfo> itemList;
}

public struct Point
{
    public string currLocation;
    public string connectedLocation;

    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }
}

public class RoomHandler : MonoBehaviour
{
    public static RoomHandler instance = null;
    public GameObject selectionCanvas;
    public Material skyboxMaterial;
    public Material defaultMaterial;
    public List<RoomInfo> roomInfoList;
    public Text debugText;
    private Dictionary<string, RoomInfo> rooomInfoContainer;
    private string currKey = null;

    public Dictionary<string, RoomInfo> RoomInfoContainer { get { return rooomInfoContainer; } }
    public string CurrKey { get { return currKey; } }

    private void Awake()
    {
        if (!instance)
            instance = this;

        rooomInfoContainer = new Dictionary<string, RoomInfo>();
        foreach (RoomInfo info in roomInfoList)
        {
            if (rooomInfoContainer.ContainsKey(info.name))
                continue;

            // Sets the gameobject active to false
            foreach (ItemInfo iter in info.itemList)
                iter.item.SetActive(false);

            rooomInfoContainer.Add(info.name, info);
        }

        RenderSettings.skybox = defaultMaterial;
    }

    // Use this for initialization
    void Start () {
	}

    // Update is called once per frame
    void Update ()
    {
        //if (Input.GetKeyDown(KeyCode.F10))
        //    ChangeLocation("Room01");
    }

    public void ChangeLocation(string key)
    {
        if (!rooomInfoContainer.ContainsKey(key))
        {
            debugText.text = "key not found";
            RenderSettings.skybox.SetTexture("_Tex", null);
            return;
        }

        // Disable ui
        selectionCanvas.SetActive(false);
        //changing of texture/go
        ChangeTexture(key);
        ChangePointOfInterest(key);
        // set the currKey after changing of texture/items
        currKey = key;
    }


    public void ShowMenu()
    {
        Debug.Log("ENTERED");
        if (selectionCanvas.activeSelf)
            return;

        RenderSettings.skybox.SetTexture("_Tex", null);
        selectionCanvas.SetActive(true);
    }

    private void ChangeTexture(string key)
    {
        // Set skybox texture 
        RenderSettings.skybox.SetTexture("_Tex", rooomInfoContainer[key].roomTexture);
        debugText.text = key;
    }

    private void ChangePointOfInterest(string key)
    {
        //enable the changed room items
        foreach (ItemInfo iter in rooomInfoContainer[key].itemList)
            iter.item.SetActive(true);
    }

    private void OnApplicationQuit()
    {
        skyboxMaterial.SetTexture("_Tex", null);
    }
}
