using System.Collections;
using System.Collections.Generic;
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
    }
}
