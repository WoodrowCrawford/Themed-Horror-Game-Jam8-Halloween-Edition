using UnityEngine;

public class ClownAttackState : ClownBaseState
{
    public override void EnterState(ClownStateManager clown)
    {
        Debug.Log("Clown is in the attack state");

        //calls the clown jumpscare event
        TimelineManager.onPlayClownJumpscare?.Invoke();

       
    }

  
    public override void UpdateState(ClownStateManager clown)
    {
        return;
    }
}
