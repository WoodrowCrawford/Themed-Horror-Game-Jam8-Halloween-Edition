using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GetInBedTriggerBehavior : MonoBehaviour
{
    public bool  playerCanGetInBed;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Player is in the area!"); 
            playerCanGetInBed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Player can not get in the bed");
            playerCanGetInBed = false;
        }
    }
}
