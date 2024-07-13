using UnityEngine;

public class ClownGettingUpState : ClownBaseState
{ 
    public override void EnterState(ClownStateManager clown)
    {
        Debug.Log("Clown is in getting up state");

        //starts the getting up animation
        clown.StartCoroutine(clown.GetUpAnimation());
       
    }

    public override void UpdateState(ClownStateManager clown)
    {
        //if the clown is fully up...
        if(clown.ClownIsUp)
        {
            //switch to the chase state
            clown.SwitchState(clown.chasePlayerState);
        }
    }
}



