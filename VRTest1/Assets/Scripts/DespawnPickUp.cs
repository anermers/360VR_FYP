using UnityEngine;
using System.Collections;

public class DespawnPickUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PickUp")
            Destroy(other.gameObject);
    }
}

