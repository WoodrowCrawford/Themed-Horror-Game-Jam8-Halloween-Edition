using UnityEngine;

public class ClownAttackState : ClownBaseState
{
    public override void EnterState(ClownStateManager clown)
    {
        if(PlayerInputBehavior.playerCanGetCaught)
        {
            Debug.Log("Clown is in the attack state");

            //calls the clown jumpscare event
            TimelineManager.onPlayClownJumpscare?.Invoke();

            //let the game know that the clown killed the player
            ClownStateManager.clownKilledPlayer = true;
        }
       
    }

  
    public override void UpdateState(ClownStateManager clown)
    {
        return;
    }
}
