using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhoulPatrolState : GhoulBaseState
{
    public GhoulStateManager ghoulStateManager;

    public override void EnterState(GhoulStateManager ghoul)
    {
       
        Debug.Log("Ghoul is in the patrol state");
        ghoul.SetSecondsToWait();
       

        ghoulStateManager = GameObject.FindGameObjectWithTag("Ghoul").GetComponent<GhoulStateManager>();
    }

    
    public override void UpdateState(GhoulStateManager ghoul)
    {
        Debug.Log("Ghoul is patrolling");

        //sets the agent speed
        ghoul.Agent.speed = 2f;

       

        ghoul.CheckIfAgentReachedDestination();
       ghoul.AnimateGhoul();
    

        if(ghoul.ghoulSightBehavior.canSeePlayer)
        {
            Debug.Log("Ghoul can see you");
            ghoul.SwitchState(ghoul.chaseState);
        }
    }

    public override void ExitState()
    {
        ghoulStateManager.StopCoroutine(ghoulStateManager.Patrol());
    }

}
