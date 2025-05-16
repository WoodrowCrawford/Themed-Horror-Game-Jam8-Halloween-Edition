using UnityEngine;
using UnityEngine.AI;

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
        Debug.Log("Ghoul is wandering");

        ghoul.Agent.speed = 1.5f;
        ghoul.AnimateGhoul();
        ghoul.CheckIfAgentReachedDestinationForWander();
    }

    public override void ExitState()
    {
        ghoulStateManager?.StopCoroutine(ghoulStateManager.WanderTimer());
        Debug.Log("Ghoul wander exit state");
    }

}
