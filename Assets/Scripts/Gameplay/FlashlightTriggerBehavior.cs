using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightTriggerBehavior : MonoBehaviour
{
    /// <summary>
    /// Trigger Behavior for flashlight. Used for the actual light dectection collider.
    /// </summary>

    [Header("Trigger Values")]
    public bool lightIsOnDummy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dummy"))
        {
            lightIsOnDummy = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        lightIsOnDummy = false;
    }
}
