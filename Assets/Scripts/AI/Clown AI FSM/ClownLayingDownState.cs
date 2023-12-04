using UnityEngine;

public class ClownLayingDownState : ClownBaseState
{
    public override void EnterState(ClownStateManager clown)
    {
        Debug.Log("clown is in the laying down state");
        
    }

    public override void UpdateState(ClownStateManager clown)
    {
        //if the jack in the box is open...
       if(clown.JackInTheBoxStateManager.jackInTheBoxOpen)
        {
            //switch to getting up state
            clown.SwitchState(clown.gettingUpState);
        } 
    }
}
