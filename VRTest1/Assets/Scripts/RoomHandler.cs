using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomInfo
{
    public string name;
    public Texture roomTexture;
    public List<GameObject> itemOfInterestList;
}

public class RoomHandler : MonoBehaviour
{
    public static RoomHandler instance = null;
    public Material skyboxMaterial;
    public List<RoomInfo> roomInfoList;
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
            return;

        // Set skybox texture 
        skyboxMaterial.SetTexture("_Tex", rooomInfoContainer[key].roomTexture);
    }
}
