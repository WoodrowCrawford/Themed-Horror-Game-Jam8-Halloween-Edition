using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClownInactiveState : ClownBaseState
{
   

    public override void EnterState(ClownStateManager clown)
    {
        Debug.Log("Clown is in inactive state");
    }

   

    public override void UpdateState(ClownStateManager clown)
    {
        if(clown.isActive)
        {
            Debug.Log("Clown is active");

            //switches the state to be the laying down state
            clown.SwitchState(clown.layingDownState);
        }
        else
        {
            Debug.Log("Clown is inactive!");
        }
    }
}
