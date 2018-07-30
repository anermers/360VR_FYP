using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GreenParticle : MonoBehaviour
{
    public GameObject particle;
    public GameObject rParticle;
    private float timer = 3.0f;
    public static bool enable;
    // Use this for initialization
    void Start()
    {
        particle = GameObject.Find("GreenEffect");
        rParticle = GameObject.Find("RedEffect");
        rParticle.SetActive(false);
        particle.SetActive(false);
        enable = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (enable)
        //{
        //    particle.SetActive(true);
        //    timer -= 1 * Time.deltaTime;
        //    if (timer <= 0)
        //    {
        //        enable = false;
        //        particle.SetActive(false);
        //        timer = 3.0f;
        //    }
        //}

        if (Input.GetKeyDown(KeyCode.T))
            PlayGreenParticle();
    }

    public void PlayGreenParticle()
    {
        StartCoroutine(DoGreenParticle());
    }

    IEnumerator DoGreenParticle()
    {
        particle.SetActive(true);
        yield return new WaitForSeconds(3f);
        particle.SetActive(false);
    }

    public void PlayRedParticle()
    {
        StartCoroutine(DoRedParticle());
    }

    IEnumerator DoRedParticle()
    {
        rParticle.SetActive(true);
        yield return new WaitForSeconds(3f);
        rParticle.SetActive(false);
    }

}
