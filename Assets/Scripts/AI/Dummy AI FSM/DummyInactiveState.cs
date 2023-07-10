using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//The inactive state for the dummy used for the day time


public class DummyInactiveState : DummyDefaultState
{
    public override void EnterState(DummyStateManager dummy)
    {
        //Testing message
        Debug.Log( dummy.gameObject.name + "Dummy is in the inactive state");

        //Make it so that the player can pick up the dummy here
        
        //Disable navmesh agent when the dummy is inactive
        dummy.Agent.enabled = false;

       
      
    }

    public override void UpdateState(DummyStateManager dummy)
    {
        //If dummy is active the switch to the laying down state
        if (dummy.isActive)
        {
            dummy.SwitchState(dummy.layingDownState);
        }
    }
}
