using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class JackInTheBoxInteractRangeBehavior : MonoBehaviour
{
    public JackInTheBoxBehavior JackInTheBoxBehavior;


    private void Awake()
    {
        JackInTheBoxBehavior= GetComponentInParent<JackInTheBoxBehavior>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            JackInTheBoxBehavior.playerCanInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            JackInTheBoxBehavior.playerCanInteract = false;
        }
    }
}
