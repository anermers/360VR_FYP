﻿using System.Collections;
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
    public GameObject selectionRoom;
    public GameObject kitchen;
    public GameObject instructionsText;
    public OVRPlayerController opc;
    public Material skyboxMaterial;
    public Material defaultMaterial;
    public List<RoomInfo> roomInfoList;
    public Text debugText;
    private Dictionary<string, RoomInfo> rooomInfoContainer;
    private string currKey = null;

    public Dictionary<string, RoomInfo> RoomInfoContainer { get { return rooomInfoContainer; } }
    public string CurrKey { get { return currKey; } }

    public Text dText;
    float dt;


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
        dt += (Time.unscaledDeltaTime - dt) * 0.1f;
        //Debug.Log(1.0f / dt);
        if(dText != null)
        dText.text = (1.0f / dt).ToString();
        //if the menu is active
        if (selectionCanvas.activeSelf)
        {
            opc.GravityModifier = 0;
            instructionsText.SetActive(false);
        }
        else
        {
            opc.GravityModifier = 0.7f;
            instructionsText.SetActive(true);
        }
        //if (Input.GetKeyDown(KeyCode.F10))
        //    ChangeLocation("Room01");
        //if (Input.GetKeyDown(KeyCode.F11))
        //    ShowMenu();
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
        selectionRoom.SetActive(false);
        //changing of texture/go
        ChangeTexture(key);
        ChangePointOfInterest(key);
        // set the currKey after changing of texture/items
        currKey = key;
        opc.GravityModifier = 0;
    }


    public void ShowMenu()
    {
        Debug.Log("ENTERED");
        RenderSettings.skybox.SetTexture("_Tex", null);
        selectionCanvas.SetActive(!selectionCanvas.activeSelf);
       // selectionRoom.SetActive(!selectionRoom.activeSelf);
       // kitchen.SetActive(!kitchen.activeSelf);
        //active the kitchen here or something
        if(kitchen.activeSelf && dText != null)
            dText.text = "active";
        opc.GravityModifier = 0.7f;
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
