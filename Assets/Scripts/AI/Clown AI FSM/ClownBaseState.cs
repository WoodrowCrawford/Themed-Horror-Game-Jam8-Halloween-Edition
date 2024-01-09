using UnityEngine;

public abstract class ClownBaseState
{
    public abstract void EnterState(ClownStateManager clown);

    public abstract void UpdateState(ClownStateManager clown);

}
