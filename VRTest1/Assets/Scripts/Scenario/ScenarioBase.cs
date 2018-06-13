﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioBase : MonoBehaviour {
    public string name;
    // bool check if the events in scneario has been completed
    protected bool isEventCompleted;
    // bool check if entire scenario has been completed
    protected bool isScenarioDone;

    public bool IsEventCompleted { get { return isEventCompleted; } set { isEventCompleted = value; } }

    protected virtual bool SwitchState(int index)
    {
        Debug.Log("Switch State");
        return true;
    }

    protected virtual void OnEnterState()
    {
        Debug.Log("Entering State");
    }
}
