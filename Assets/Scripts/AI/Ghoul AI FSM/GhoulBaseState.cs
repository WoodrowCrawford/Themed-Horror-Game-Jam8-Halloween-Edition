using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GhoulBaseState
{
    public delegate void Exit();
    public static Exit onSwitchState;

    public abstract void EnterState(GhoulStateManager ghoul);

    public abstract void UpdateState(GhoulStateManager ghoul);

    public abstract void ExitState();
}
