using UnityEngine;

public class GhoulInactiveState : GhoulBaseState
{
    public override void EnterState(GhoulStateManager ghoul)
    {
        Debug.Log("Ghoul is in the inactive state");
      
    }

   

    public override void UpdateState(GhoulStateManager ghoul)
    {
        if(ghoul.isActive)
        {
            ghoul.SwitchState(ghoul.patrolState);
        }
        else
        {
            Debug.Log("Ghoul is inactive!");
        }
    } 
    
    public override void ExitState()
    {
        Debug.Log("ghoul inactive exit state");
    }
}
