using UnityEngine;

public class WindowDisabledState : WindowBaseState
{
    public override void EnterState(WindowStateManager window)
    {
        //testing
        Debug.Log("Window is in the disabled state");
    }

 
    public override void UpdateState(WindowStateManager window)
    {
        //if the window is active...
        if (window.isActive)
        {
            //switch to the waiting state
            window.SwitchState(window.waitingState);
        }
    } 
    

    public override void ExitState()
    {
        Debug.Log("Window disabled state exit");
    }
}
