using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCollider : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        RunnerGameManager.Instance.objectPickupManager.AddPickup(other.gameObject);
    }
}
