using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The first state the dummy will be in when activated
/// </summary>

public class DummyLayingDownState : DummyDefaultState
{
    public override void EnterState(DummyStateManager dummy)
    {
        //Testing message
        Debug.Log("Dummy is in the laying down state");

        //Starts the startup phase
        dummy.StartCoroutine(dummy.InitiateStartUp());
    }

    public override void UpdateState(DummyStateManager dummy)
    {
        if(dummy.initiatePhaseComplete)
        {
            //Switch to getting up phase
            dummy.SwitchState(dummy.gettingUpState);
        }

    }


   
   
}
