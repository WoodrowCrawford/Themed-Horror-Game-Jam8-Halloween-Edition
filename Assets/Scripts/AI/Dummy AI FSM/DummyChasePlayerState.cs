using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DummyChasePlayerState : DummyDefaultState
{
    public override void EnterState(DummyStateManager dummy)
    {
        Debug.Log(dummy.dummyThisBelongsTo.gameObject.name +  " is in the chase player state");
    }

    public override void UpdateState(DummyStateManager dummy)
    {
        //if dummy is hit with a light...
        if (dummy.dummyThisBelongsTo.GetComponent<DummyStateManager>().dummyIsHitWithLight)
        {
            //Switch state to run away state
            dummy.dummyThisBelongsTo.GetComponent<DummyStateManager>().SwitchState(dummy.runAwayState);
            
        }
        else if(!dummy.dummyThisBelongsTo.GetComponent<DummyStateManager>().dummyIsHitWithLight)
        {
            //Else set the dummy to chase the player
            ChaseTarget(dummy);
        }

        //If player is in range...
        //switch to the attack state
    }

    
    //A function used to chase the current target
    public void ChaseTarget(DummyStateManager dummy)
    {

        //If player is in bed...
        if (dummy.PlayerRef.GetComponent<PlayerInputBehavior>().playerControls.InBed.enabled)
        {
            //Set target to out of bed target (changes for each dummy)
            
            dummy.gameObject.GetComponent<DummyStateManager>().target = dummy.InBedTarget;
            
        }
        else
        {
            //Set target to be the player
            dummy.gameObject.GetComponent<DummyStateManager>().target = dummy.PlayerRef;
        }

        //Chases target
        dummy.Agent.SetDestination(dummy.target.transform.position);

        //If player is in range.... attack (switch to attack state)

    }
   
}
