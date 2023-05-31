using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhoulChaseState : GhoulBaseState
{
    public override void EnterState(GhoulStateManager ghoul)
    {
        Debug.Log("Ghoul is in the chase state");
       
    }

    public override void UpdateState(GhoulStateManager ghoul)
    {
        Vector3 playerPosition = new Vector3(ghoul.PlayerRef.transform.position.x, ghoul.transform.position.y, ghoul.PlayerRef.transform.position.z);

        ghoul.Animator.SetFloat("Speed", ghoul.agent.velocity.magnitude);

        //Set the destination to be the player
        ghoul.agent.SetDestination(ghoul.PlayerRef.transform.position);

        //Set the agent speed
        ghoul.agent.speed = 4f;

        //used to make the ai look at the player position
        ghoul.transform.LookAt(playerPosition);

        if(!ghoul.ghoulSightBehavior.canSeePlayer)
        {
            ghoul.SwitchState(ghoul.patrolState);
        }
    }
}
