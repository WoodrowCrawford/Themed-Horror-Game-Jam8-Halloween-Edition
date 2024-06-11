using UnityEngine;


public class DummyAttackState : DummyDefaultState
{
   
    public override void EnterState(DummyStateManager dummy)
    {
        //first check if the player can be attacked
        if(PlayerInputBehavior.playerCanGetCaught)
        {
            Debug.Log("dummy attacked!");

            //call the dummy jumpscare event
            TimelineManager.onPlayDummyJumpscare?.Invoke();

            //signal an event that lets the game know that the dummy attacked
            DummyStateManager.dummyKilledPlayer = true;
        }
  
    }

    public override void UpdateState(DummyStateManager dummy)
    {
        return;
    }
}
