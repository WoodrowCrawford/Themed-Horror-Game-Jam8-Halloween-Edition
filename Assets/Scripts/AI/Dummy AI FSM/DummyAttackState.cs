using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyAttackState : DummyDefaultState
{
    public override void EnterState(DummyStateManager dummy)
    {
        Debug.Log("Dummy is in the attack state");

       
    }

    public override void UpdateState(DummyStateManager dummy)
    {
       
    }

   
}
