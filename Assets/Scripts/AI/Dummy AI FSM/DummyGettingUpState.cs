using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyGettingUpState : DummyDefaultState
{
    public override void EnterState(DummyStateManager dummy)
    {
        Debug.Log(dummy.dummyThisBelongsTo.name + " is getting up! yeah");

        //Set the movement speed for the dummy
        dummy.SetMovementSpeed();

        dummy.StartCoroutine(dummy.DummyGetUp());

        //set the values for the dummy getting up state
    }

    public override void UpdateState(DummyStateManager dummy)
    {
        //if dummy is up...
        if(dummy.isDummyUp)
        {
            

            //switch to the chase player state
            dummy.SwitchState(dummy.chasePlayerState);
        }
    }

   
    

}
