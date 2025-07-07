using UnityEngine;

public class DummyRunAwayState : DummyDefaultState
{
    public override void EnterState(DummyStateManager dummy)
    {
        Debug.Log( dummy.dummyThisBelongsTo.gameObject.name +  "Dummy is in the run away state");
    }

    public override void UpdateState(DummyStateManager dummy)
    {
        //make the dummy run away to the origin position
        dummy.dummyThisBelongsTo.GetComponent<DummyStateManager>().Agent.SetDestination(dummy.dummyThisBelongsTo.GetComponent<DummyStateManager>().OriginPos.transform.position);

        //Sets the animators speed to equal the agents speed
        dummy.Animator.SetFloat("Speed", dummy.Agent.velocity.magnitude);

        //if the dummy is at the origin position
        if(dummy.dummyThisBelongsTo.GetComponent<DummyStateManager>().dummyIsAtOrigin)
        {
            //Switch ai state to be laying back down phase
            dummy.dummyThisBelongsTo.GetComponent<DummyStateManager>().SwitchState(dummy.layBackDownState);
        }


        //if the dummy is no longer hit with light or if the light is turned off
       else  if (!dummy.dummyThisBelongsTo.GetComponent<DummyStateManager>().dummyIsHitWithLight)
        {
            dummy.SwitchState(dummy.chasePlayerState);
        }
    }
}
