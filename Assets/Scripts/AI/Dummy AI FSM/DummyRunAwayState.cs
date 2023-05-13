using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyRunAwayState : DummyDefaultState
{
    public override void EnterState(DummyStateManager dummy)
    {
        Debug.Log( dummy.dummyThisBelongsTo.gameObject.name +  "Dummy is in the run away state");
    }

    public override void UpdateState(DummyStateManager dummy)
    {


        //if the dummy is no longer hit with light or if the light is turned off
       if(!dummy.dummyThisBelongsTo.GetComponent<DummyStateManager>().dummyIsHitWithLight)
        {
            dummy.SwitchState(dummy.chasePlayerState);
        }

       
    }

    
}
