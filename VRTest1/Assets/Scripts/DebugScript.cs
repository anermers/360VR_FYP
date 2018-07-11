using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugScript : MonoBehaviour {

    public Text m_text;

    private void Update()
    {
        m_text.text = transform.eulerAngles.z.ToString();
    }

    //void Start()
    //{
    //    OVRTouchpad.Create();
    //    OVRTouchpad.TouchHandler += HandleTouchHandler;
    //}

    //public void HandleTouchHandler(object sender, System.EventArgs e)
    //{
    //    var touchArgs = (OVRTouchpad.TouchArgs)e;
    //    if (touchArgs.TouchType == OVRTouchpad.TouchEvent.SingleTap)
    //    {
    //        m_text.text = "Received SingleTap";
    //        //Debug.Log("Received SingleTap");
    //    }
    //    else
    //    {
    //        m_text.text = "Received " + touchArgs.TouchType;
    //        //Debug.Log("Received " + e);
    //    }
    //}
}

