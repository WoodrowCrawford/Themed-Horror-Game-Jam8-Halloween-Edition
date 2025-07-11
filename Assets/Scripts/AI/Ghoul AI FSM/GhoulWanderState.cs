using UnityEngine;

public class GhoulWanderState : GhoulBaseState
{
    public GhoulStateManager ghoulStateManager;

    public override void EnterState(GhoulStateManager ghoul)
    { 
        Debug.Log("Ghoul is wandering");
        ghoul.StartCoroutine(ghoul.WanderTimer());
      
        ghoulStateManager = Object.FindFirstObjectByType<GhoulStateManager>();
    }

    public override void UpdateState(GhoulStateManager ghoul)
    {
        ghoul.Agent.speed = 1f;
        ghoul.AnimateGhoul();
        ghoul.CheckIfAgentReachedDestinationForWander();

        if (ghoul.ghoulSightBehavior.canSeePlayer)
        {
            ghoul.SwitchState(ghoul.chaseState);
        }
    }

    public override void ExitState()
    {
        ghoulStateManager?.StopCoroutine(ghoulStateManager.WanderTimer());
        Debug.Log("Ghoul wander exit state");
    }

}
