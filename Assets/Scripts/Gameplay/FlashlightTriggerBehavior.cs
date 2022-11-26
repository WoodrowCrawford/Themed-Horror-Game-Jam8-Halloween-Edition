using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
            //Change the target the light hits the dummy
            dummyBehavior.target = dummyBehavior.dummyOrigin.gameObject;

            //Changes the speed of the dummy when the light hits it
            dummyBehavior.agent.speed = 4f;
        }
       
    }

    private void OnTriggerExit(Collider other)
    {
        dummyBehavior.target = dummyBehavior._playerRef.gameObject;
        dummyBehavior.agent.speed = 0.5f;
    }





}
