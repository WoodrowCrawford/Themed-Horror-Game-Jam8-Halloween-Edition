using UnityEngine;

public class GhoulPatrolState : GhoulBaseState
{
    public GhoulStateManager ghoulStateManager;

    public override void EnterState(GhoulStateManager ghoul)
    {
        Debug.Log("Ghoul is in the patrol state");
   
        ghoul.SetSecondsToWait();

        ghoulStateManager  = Object.FindFirstObjectByType<GhoulStateManager>();

        //sets the agent speed
        ghoul.Agent.speed = 2f;
    }

    
    public override void UpdateState(GhoulStateManager ghoul)
    {
        ghoul.CheckIfAgentReachedDestination();
        ghoul.AnimateGhoul();


        if (ghoul.ghoulSightBehavior.canSeePlayer)
        {
            ghoul.SwitchState(ghoul.chaseState);
        }

    }

    public override void ExitState()
    {
        return;
    }

}
