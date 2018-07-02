using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetBurn : MonoBehaviour {

    [Header("Chef skeleton")]
    public Transform leftHand;
    [Header("Chef interactable object")]
    public Transform pan;

    private Animator chefAnim;
    private bool isMoveBack;

    // Use this for initialization
    void Start () {
        isMoveBack = false;
        chefAnim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {

        if(isMoveBack)
        {
            if (transform.localPosition.z < 0.3f)
                transform.position += new Vector3(0, 0, 1) * 0.8f * Time.deltaTime;
            else
                this.enabled = false;
        }

    }

    AnimationClip CurrentAnimationClip()
    {
        //var currAnimName = "Default";
        foreach (AnimationClip clip in chefAnim.runtimeAnimatorController.animationClips)
        {
            if (chefAnim.GetCurrentAnimatorStateInfo(0).IsName(clip.name))
                return clip;
        }
        return null;
    }

    void GrabPan()
    {
        Debug.Log("Pan grabbed");
        pan.parent = leftHand;

        if (pan.GetComponent<Rigidbody>() == null)
            pan.gameObject.AddComponent<Rigidbody>();
        if (pan.GetComponent<BoxCollider>() == null)
            pan.gameObject.AddComponent<BoxCollider>();
    }

    void ReleasePan()
    {
        Debug.Log("Pan released");
        pan.parent = null;
    }

    void BurnMoveBack()
    {
        Debug.Log("Move back");
        isMoveBack = true;
    }
}
