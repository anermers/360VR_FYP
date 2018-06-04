using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        if (lineRenderer != null) {
            lineRenderer.enabled = trackingSpace != null && activeController != OVRInput.Controller.None;
        }
    }

    Ray UpdateCastRayIfPossible() {
        if (trackingSpace != null && activeController != OVRInput.Controller.None) {
            Quaternion orientation = OVRInput.GetLocalControllerRotation(activeController);
            Vector3 localStartPoint = OVRInput.GetLocalControllerPosition(activeController);

            Matrix4x4 localToWorld = trackingSpace.localToWorldMatrix;
            Vector3 worldStartPoint = localToWorld.MultiplyPoint(localStartPoint);
            Vector3 worldOrientation = localToWorld.MultiplyVector(orientation * Vector3.forward);
            
            if (lineRenderer != null) {
                lineRenderer.SetPosition(0, worldStartPoint);
                lineRenderer.SetPosition(1, worldStartPoint + worldOrientation * rayLength);
            }

            if (inputModule != null) {
                inputModule.SelectionRay = new Ray(worldStartPoint, worldOrientation);
                return inputModule.SelectionRay;
            }
        }

        return new Ray();
    }

	void Update () {
        DetermineActiveController();
        DisableLineRendererIfNeeded();
        Ray selectionRay = UpdateCastRayIfPossible();

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
    }
}
