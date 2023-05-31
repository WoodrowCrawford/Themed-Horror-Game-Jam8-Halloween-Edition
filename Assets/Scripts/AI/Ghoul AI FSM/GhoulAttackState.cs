using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhoulAttackState : GhoulBaseState
{
    public override void EnterState(GhoulStateManager ghoul)
    {
        Debug.Log("Ghoul is in the attack state");
    }

    public override void UpdateState(GhoulStateManager ghoul)
    {
       
    }
}
