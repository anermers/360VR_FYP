using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkyboxTextureInfo
{
    public string key;
    public Texture value;
}

public class SkyboxTextureHandler : MonoBehaviour
{

    public static SkyboxTextureHandler instance = null;
    public Material skyboxMaterial;
    public List<SkyboxTextureInfo> textureInfoList; 
    private Dictionary<string, Texture> textureContainer;

    private void Awake()
    {
        if (!instance)
            instance = this;
    }

    // Use this for initialization
    void Start ()
    {
        textureContainer = new Dictionary<string, Texture>();
        
        // Adds item in list to the dictionary
        foreach (SkyboxTextureInfo info in textureInfoList)
        {
            if (textureContainer.ContainsKey(info.key))
                continue;

            textureContainer.Add(info.key, info.value);
        }

    }
	
	// Update is called once per frame
	void Update ()
    {
    }

    public void ChangeTexture(string key)
    {
        if (!textureContainer.ContainsKey(key))
            return;

        // Set skybox texture 
        skyboxMaterial.SetTexture("_Tex", textureContainer[key]);
    }


}
