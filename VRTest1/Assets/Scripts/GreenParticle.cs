using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GreenParticle : MonoBehaviour
{
    public GameObject particle;
    private float timer = 3.0f;
    public static bool enable;
    // Use this for initialization
    void Start()
    {
        particle = GameObject.Find("GreenEffect");
        particle.SetActive(false);
        enable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(enable)
        {
            particle.SetActive(true);
            timer -= 1 * Time.deltaTime;
            if(timer <=0)
            {
                enable = false;
                particle.SetActive(false);
                timer = 3.0f;
            }
        }
    }
}
