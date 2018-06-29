using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetBurn : MonoBehaviour {

    private Animator chefAnim;

	// Use this for initialization
	void Start () {
        chefAnim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        Debug.Log(currentAnimationName());
    }

    string currentAnimationName()
    {
        var currAnimName = "NO NAME";
        foreach (AnimationClip clip in chefAnim.runtimeAnimatorController.animationClips)
            if (chefAnim.GetCurrentAnimatorStateInfo(0).IsName(clip.name))
                currAnimName = clip.name.ToString();
     
        return currAnimName;
    }
}
