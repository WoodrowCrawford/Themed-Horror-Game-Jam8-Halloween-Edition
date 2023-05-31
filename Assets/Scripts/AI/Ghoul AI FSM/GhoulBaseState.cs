using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GhoulBaseState
{
    public abstract void EnterState(GhoulStateManager ghoul);

    public abstract void UpdateState(GhoulStateManager ghoul);
}
