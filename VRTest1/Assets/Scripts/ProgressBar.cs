using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {
    public const float pValue = 100f;
    public float currValue = pValue;
    public Image img;
    private float fAmount = 0f;

    void Start()
    {
        fAmount = Mathf.Lerp(fAmount, currValue / pValue, currValue);
        img.fillAmount = fAmount;
    }

    // Update is called once per frame
//    void Update () {
//#if UNITY_EDITOR
//        if (Input.GetKeyDown(KeyCode.K))
//            AddProgress(25f);
//#endif
//    }

    public void AddProgress(float amount)
    {
        currValue += amount;
        if (currValue <= 0)
            currValue = 0;

        fAmount = Mathf.Lerp(fAmount, currValue / pValue, Mathf.Abs(amount));
        img.fillAmount = fAmount;
    }
}
