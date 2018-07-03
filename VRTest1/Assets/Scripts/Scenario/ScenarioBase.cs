using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioBase : MonoBehaviour {

    public string name;
    public Transform chefSpawnPoint;
    // bool check if the events in scneario has been completed
    protected bool isEventCompleted;
    // bool check if entire scenario has been completed
    protected bool isScenarioDone;
    // bool check if interact with object....
    protected bool isInteracted;
    // list of all the instructions
    protected List<string> allInstructions;

    // GameObject that is interacted
    [HideInInspector]
    public GameObject InteractedGO;
    // use to determine which step the user is at 
    [HideInInspector]
    public int step;

    public bool IsScenarioDone { get { return isScenarioDone; } }
    public bool IsEventCompleted { get { return isEventCompleted; } set { isEventCompleted = value; } }
    public bool IsInteracted { get { return isInteracted; } set { isInteracted = value; } }
    public List<string> AllInstructions { get { return allInstructions; } }

    public virtual void Init()
    {
        Debug.Log("Init to be called ONCE in handler");
    }

    protected virtual bool SwitchState(int index)
    {
        Debug.Log("Switch State");
        return true;
    }

    protected virtual void OnEnterState()
    {
        Debug.Log("Entering State");
    }

    protected virtual void SetCurrentInteractable()
    {
        Debug.Log("Set interactables");
    }
}
