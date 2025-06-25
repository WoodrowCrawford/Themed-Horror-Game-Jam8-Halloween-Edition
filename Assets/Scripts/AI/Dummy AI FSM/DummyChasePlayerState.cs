using UnityEngine;

public class DummyChasePlayerState : DummyDefaultState
{
    public override void EnterState(DummyStateManager dummy)
    {
        
        //Testing 
        Debug.Log(dummy.dummyThisBelongsTo.gameObject.name +  " is in the chase player state");
    }

    public override void UpdateState(DummyStateManager dummy)
    {
        //Sets the animators speed to equal the agents speed
        dummy.dummyThisBelongsTo.GetComponent<DummyStateManager>().Animator.SetFloat("Speed", dummy.Agent.velocity.magnitude);

        //if dummy is hit with a light...
        if (dummy.dummyThisBelongsTo.GetComponent<DummyStateManager>().dummyIsHitWithLight)
        {
            //Switch state to run away state
            dummy.dummyThisBelongsTo.GetComponent<DummyStateManager>().SwitchState(dummy.runAwayState);

        }
        else if(!dummy.dummyThisBelongsTo.GetComponent<DummyStateManager>().dummyIsHitWithLight)
        {
            //Else set the dummy to keep chasing the player
            ChaseTarget(dummy.dummyThisBelongsTo.GetComponent<DummyStateManager>());
        }

        //checks if the dummy is in range
        dummy.CheckIfTargetIsInRange();
    
    }

    
    //A function used to chase the current target
    public void ChaseTarget(DummyStateManager dummy)
    {

        //If player is in bed...
        if (PlayerInputBehavior.inBed)
        {
            //Set target to out of bed target (changes for each dummy)
            dummy.gameObject.GetComponent<DummyStateManager>().Target = dummy.InBedTarget;
            
        }
        else
        {
            //Set target to be the player
            dummy.gameObject.GetComponent<DummyStateManager>().Target = dummy.PlayerRef;
        }

        //Chases target
        dummy.Agent.SetDestination(dummy.Target.transform.position);

        //If player is in range.... attack (switch to attack state)

    }
   
}
