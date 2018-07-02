using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class RayPointer : MonoBehaviour {
    [System.Serializable]
    public class HoverCallback : UnityEvent<Transform> { }
    [System.Serializable]
    public class SelectionCallback : UnityEvent<Transform> { }

    [Header("Pointer Config")]
    public bool interactWithNonUIObjects = true;
    public LayerMask nonUIExcludeLayers;
    public float rayLength = 500;
    public Transform trackingSpace = null;
    public LineRenderer lineRenderer = null;
    public UnityEngine.EventSystems.OVRInputModule inputModule = null;

    [Header("Non UI Hover Callbacks")]
    public RayPointer.HoverCallback onHoverEnter;
    public RayPointer.HoverCallback onHoverExit;
    public RayPointer.HoverCallback onHover;

    [Header("Non UI Selection Callbacks")]
    public RayPointer.SelectionCallback onNonUISelected;

    protected OVRInput.Controller activeController = OVRInput.Controller.RTrackedRemote;
    public OVRInput.Button joyPadClickButton = OVRInput.Button.PrimaryIndexTrigger;

    protected Transform lastHit = null;
    protected Transform triggerDown = null;

    [Header("Other modifiers")]
    public GameObject kitchenModel;
    public GameObject testPoint;
    public GameObject instructionMenu;
    public Camera CentreEyeCamera;
    public bool isController = true;
    public Vector3 wPoint;

    private Vector3 DefaultPos;
    private LineRenderer lineReference;

    void Awake () {
        if (inputModule != null) {
            inputModule.OnSelectionRayHit += RayHitSomething;
            joyPadClickButton = inputModule.joyPadClickButton;
        }

        OVRInput.Controller controller = OVRInput.GetConnectedControllers();

        if ((controller & OVRInput.Controller.RTouch) == OVRInput.Controller.RTouch) {
            activeController = OVRInput.Controller.RTouch;
        }

        if ((controller & OVRInput.Controller.LTouch) == OVRInput.Controller.LTouch) {
            activeController = OVRInput.Controller.LTouch;
        }

        if ((controller & OVRInput.Controller.RTrackedRemote) == OVRInput.Controller.RTrackedRemote) {
            activeController = OVRInput.Controller.RTrackedRemote;
        }

        if ((controller & OVRInput.Controller.LTrackedRemote) == OVRInput.Controller.LTrackedRemote) {
            activeController = OVRInput.Controller.LTrackedRemote;
        }

        //set defaultPos
        DefaultPos = new Vector3(0,0,0);
        DefaultPos = transform.position;

        //set active state of point for the raycast from centre eye
        if(testPoint != null)
            testPoint.SetActive(!isController);

        lineReference = lineRenderer;
    }

    void OnDestroy() {
        if (inputModule != null) {
            inputModule.OnSelectionRayHit -= RayHitSomething;
        }
    }

    void RayHitSomething(Vector3 hitPosition, Vector3 hitNormal) {
        if (lineRenderer != null) {
            lineRenderer.SetPosition(1, hitPosition);
        }
    }

    void DetermineActiveController() {
        OVRInput.Controller controller = OVRInput.GetConnectedControllers();

        if ((controller & OVRInput.Controller.RTouch) == OVRInput.Controller.RTouch) {
            if (OVRInput.Get(joyPadClickButton, OVRInput.Controller.RTouch)) {
                activeController = OVRInput.Controller.RTouch;
                return;
            }
        }

        if ((controller & OVRInput.Controller.LTouch) == OVRInput.Controller.LTouch) {
            if (OVRInput.Get(joyPadClickButton, OVRInput.Controller.LTouch)) {
                activeController = OVRInput.Controller.LTouch;
                return;
            }
        }

        if ((controller & OVRInput.Controller.RTrackedRemote) == OVRInput.Controller.RTrackedRemote) {
            if (OVRInput.Get(joyPadClickButton, OVRInput.Controller.RTrackedRemote)) {
                activeController = OVRInput.Controller.RTrackedRemote;
                return;
            }
        }

        if ((controller & OVRInput.Controller.LTrackedRemote) == OVRInput.Controller.LTrackedRemote) {
            if (OVRInput.Get(joyPadClickButton, OVRInput.Controller.LTrackedRemote)) {
                activeController = OVRInput.Controller.LTrackedRemote;
                return;
            }
        }

        if ((controller & activeController) != activeController) {
            activeController = OVRInput.Controller.None;
        }

        if (inputModule != null) {
            inputModule.activeController = activeController;
        }
    }

    void DisableLineRendererIfNeeded() {
        if (lineRenderer != null && isController) {
            lineRenderer.enabled = trackingSpace != null && activeController != OVRInput.Controller.None;
        }
    }

    Ray UpdateCastRayIfPossible() {
        if (trackingSpace != null /*&& activeController != OVRInput.Controller.None*/) {

            Quaternion orientation = new Quaternion();
            Vector3 localStartPoint = new Vector3();
            
            if(!isController)
            {
                orientation = CentreEyeCamera.transform.rotation;
                localStartPoint = CentreEyeCamera.transform.localPosition;
            }
            else
            {
                orientation = OVRInput.GetLocalControllerRotation(activeController);
                localStartPoint = OVRInput.GetLocalControllerPosition(activeController);
            }


            Matrix4x4 localToWorld = trackingSpace.localToWorldMatrix;
            Vector3 worldStartPoint = localToWorld.MultiplyPoint(localStartPoint);
            Vector3 worldOrientation = localToWorld.MultiplyVector(orientation * Vector3.forward);
            wPoint = worldOrientation;

            if (lineRenderer != null) {
                lineRenderer.SetPosition(0, worldStartPoint);
                lineRenderer.SetPosition(1, worldStartPoint + worldOrientation * rayLength);
            }

            if (inputModule != null) {
                inputModule.SelectionRay = new Ray(worldStartPoint, worldOrientation);
                return inputModule.SelectionRay;
            }

            if(!isController && testPoint != null)
                testPoint.transform.localPosition = worldStartPoint + worldOrientation;
        }
      
        return new Ray();
    }

	void Update () {
        DetermineActiveController();
        DisableLineRendererIfNeeded();
        Ray selectionRay = UpdateCastRayIfPossible();

        if (!isController && testPoint != null)
            testPoint.transform.position = transform.position + wPoint;

        if (OVRInput.Get(OVRInput.Button.Back, activeController) ||
        Input.GetKeyDown(KeyCode.F6))
        {
            //if (kitchenModel.activeInHierarchy)
            //    kitchenModel.SetActive(false);

            //RoomHandler.instance.ShowMenu();
            //if(RoomHandler.instance.CurrKey != null)
            //{
            //    foreach (ItemInfo iter in RoomHandler.instance.RoomInfoContainer[RoomHandler.instance.CurrKey].itemList)
            //        iter.item.SetActive(false);
            //}

            //ScenarioHandler.instance.ScenarioQuit();
            //transform.position = DefaultPos;

            //reload the curr scene agn (wow magic)
            Scene currScene =  SceneManager.GetActiveScene();
            SceneManager.LoadScene(currScene.name);
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F11))
            //ScenarioHandler.instance.ScenarioFireSelect(true);
            ScenarioHandler.instance.SelectScenarioType("sc");
        if (Input.GetKeyDown(KeyCode.F12))
            ScenarioHandler.instance.RandomScenarioType();
#endif

        if (OVRInput.Get(OVRInput.Button.DpadUp, activeController) ||
            Input.GetKeyDown(KeyCode.Backspace))
        {
            isController = !isController;
            lineRenderer.enabled = isController;
            if (testPoint != null)
                testPoint.SetActive(!isController);
        }

        if(OVRInput.Get(OVRInput.Button.DpadDown, activeController) || 
            Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if(kitchenModel.activeSelf)
                instructionMenu.GetComponent<InstructionMenu>().DisplayInstructionMenu();
        }

        if (interactWithNonUIObjects) {
            ProcessNonUIInteractions(selectionRay);
        }

    }

    void ProcessNonUIInteractions(Ray pointer) {

        RaycastHit hit; // Was anything hit?
        if (Physics.Raycast(pointer, out hit, rayLength, ~nonUIExcludeLayers)) {

            if (lastHit != null && lastHit != hit.transform) {
                if (onHoverExit != null) {
                    onHoverExit.Invoke(lastHit);
                }
                lastHit = null;
            }

            if (lastHit == null) {
                if (onHoverEnter != null) {
                    onHoverEnter.Invoke(hit.transform);
                }
            }

            if (onHover != null) {
                onHover.Invoke(hit.transform);
            }

            lastHit = hit.transform;

            // Handle selection callbacks. An object is selected if the button selecting it was
            // pressed AND released while hovering over the object.
            if (activeController != OVRInput.Controller.None) {
                if (OVRInput.GetDown(joyPadClickButton, activeController)) {
                         triggerDown = lastHit;
                }
                else if (OVRInput.GetUp(joyPadClickButton, activeController)) {
                    if (triggerDown != null && triggerDown == lastHit) {
                        if (onNonUISelected != null) {
                            onNonUISelected.Invoke(triggerDown);
                        }
                    }
                }

                if (!OVRInput.Get(joyPadClickButton, activeController)) {
                    triggerDown = null;
                }
            }
        }
        // Nothing was hit, handle exit callback
        else if (lastHit != null) {
            if (onHoverExit != null) {
                onHoverExit.Invoke(lastHit);
            }
            lastHit = null;
        }

#if UNITY_EDITOR
        if (activeController == OVRInput.Controller.None)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                triggerDown = lastHit;
            }
            else if (Input.GetKeyUp(KeyCode.L))
            {
                if (triggerDown != null && triggerDown == lastHit)
                {
                    if (onNonUISelected != null)
                    {
                        onNonUISelected.Invoke(triggerDown);
                    }
                }
            }

            if (!Input.GetKey(KeyCode.L))
            {
                triggerDown = null;
            }
        }
        // Nothing was hit, handle exit callback
        else if (lastHit != null) {
            if (onHoverExit != null) {
                onHoverExit.Invoke(lastHit);
            }
            lastHit = null;
        }

#endif


    }
}


/*
 * pointer to point in direction of the obj ti interact
 * reduce text info size
 * enable/disable the instruction (swipe the touch-pad)
 *  > pop up a menu showing the next objective instructions (if look too far away from the menu, it snaps 
 *  back to front of user) cards infront of the user
 *  Stuff in medkit falls through ground
 */