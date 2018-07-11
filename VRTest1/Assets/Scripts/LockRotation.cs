using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotation : MonoBehaviour {

    public bool isLockX = false;
    public bool isLockY = false;
    public bool isLockZ = true;

    private float xLock = 0;
    private float yLock = 0;
    private float zLock = 0;

	// Update is called once per frame
	void LateUpdate () {

        xLock = isLockX ? 0 : transform.rotation.eulerAngles.x;
        yLock = isLockY ? 0 : transform.rotation.eulerAngles.y;
        zLock = isLockZ ? 0 : transform.rotation.eulerAngles.z;

        transform.rotation = Quaternion.Euler(xLock, yLock, zLock);
    }
}
