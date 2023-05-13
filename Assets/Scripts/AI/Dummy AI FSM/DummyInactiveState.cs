using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyInactiveState : DummyDefaultState
{
    public override void EnterState(DummyStateManager dummy)
    {
        //Testing message
        Debug.Log( dummy.gameObject.name + "Dummy is in the inactive state");

        
        
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
