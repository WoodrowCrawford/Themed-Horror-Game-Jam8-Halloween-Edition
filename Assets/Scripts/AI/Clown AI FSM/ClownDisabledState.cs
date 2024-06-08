using Unity;
using UnityEngine;

public class ClownDisabledState : ClownBaseState
{
    public override void EnterState(ClownStateManager clown)
    {
        Debug.Log("Clown is in the disabled state");
        
    }

    public override void UpdateState(ClownStateManager clown)
    {
        return;
    }

}
