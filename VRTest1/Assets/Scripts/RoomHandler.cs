using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class RoomInfo
{
    public string name;
    public Texture roomTexture;
    public List<GameObject> itemList;
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

    private void Awake()
    {
        if (!instance)
            instance = this;

        rooomInfoContainer = new Dictionary<string, RoomInfo>();
        foreach (RoomInfo info in roomInfoList)
        {
            if (rooomInfoContainer.ContainsKey(info.name))
                continue;
            rooomInfoContainer.Add(info.name, info);

        }

        //RenderSettings.skybox = defaultMaterial;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
	}

    public void ChangeTexture(string key)
    {
        if (!rooomInfoContainer.ContainsKey(key))
        {
            debugText.text = "key not found";
            RenderSettings.skybox.SetTexture("_Tex", null);
            return;
        }

        // Set skybox texture 
        RenderSettings.skybox.SetTexture("_Tex", rooomInfoContainer[key].roomTexture);
        // Disable ui
        //selectionCanvas.SetActive(false);
        debugText.text = key;
    }

    private void OnApplicationQuit()
    {
        //skyboxMaterial.SetTexture("_Tex", null);
    }
}
