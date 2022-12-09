using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WardrobeDoorTriggerBehavior : MonoBehaviour
{
    public bool playerCanOpenWardrobe;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerCanOpenWardrobe= true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerCanOpenWardrobe= false;
        }

       
    }
}
