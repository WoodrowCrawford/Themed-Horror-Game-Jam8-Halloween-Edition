using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class FlashlightTriggerBehavior : MonoBehaviour
{
    /// <summary>
    /// Trigger Behavior for flashlight. Used for the actual light dectection collider.
    /// </summary>

    public DummyBehavior dummyBehavior;

    [Header("Trigger Values")]
    public bool lightIsOnDummy;


    private void Awake()
    {
        dummyBehavior = GameObject.FindGameObjectWithTag("Dummy").GetComponent<DummyBehavior>();
    }

    private void OnTriggerStay(Collider other)
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
