using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyLayBackDownState : DummyDefaultState
{
    public override void EnterState(DummyStateManager dummy)
    {
        Debug.Log("Dummy is in the lay back down state");
    }

    public override void UpdateState(DummyStateManager dummy)
    {
        
    }

   
}
