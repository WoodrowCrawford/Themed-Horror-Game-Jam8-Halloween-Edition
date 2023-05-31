using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhoulPatrolState : GhoulBaseState
{
    public override void EnterState(GhoulStateManager ghoul)
    {
        Debug.Log("Ghoul is in the patrol state");
        ghoul.SetSecondsToWait();
        ghoul.StartCoroutine(ghoul.Patrol());
    }

    public override void UpdateState(GhoulStateManager ghoul)
    {
        ghoul.Animator.SetFloat("Speed", ghoul.agent.velocity.magnitude);


        if(ghoul.ghoulSightBehavior.canSeePlayer)
        {
            Debug.Log("Ghoul can see you");
            ghoul.SwitchState(ghoul.chaseState);
        }
    }
}
