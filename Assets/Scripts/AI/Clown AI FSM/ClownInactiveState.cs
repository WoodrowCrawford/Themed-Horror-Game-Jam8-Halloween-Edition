using UnityEngine;

public class ClownInactiveState : ClownBaseState
{
    public override void EnterState(ClownStateManager clown)
    {
        Debug.Log("Clown is in inactive state");
        clown.InteractionPrompt = "Examine";
        clown.highlightBehavior.isActive = true;
        
    }


    public override void UpdateState(ClownStateManager clown)
    {
        if(clown.IsActive)
        {
            //switches the state to be the laying down state
            clown.SwitchState(clown.layingDownState);
        }
        else
        {
            return;
        }
    }
}
