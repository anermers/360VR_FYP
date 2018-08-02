using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour {

    public float timer = 3.0f;
    public bool isEnabled = false;

	// Use this for initialization
	void Start () {
        gameObject.SetActive(isEnabled);
	}

    public void PlayParticle()
    {
        gameObject.SetActive(true);
        StartCoroutine(DoParticle());
    }

    IEnumerator DoParticle()
    {
        gameObject.SetActive(true);
        yield return new WaitForSeconds(timer);
        gameObject.SetActive(false);
    }
}
