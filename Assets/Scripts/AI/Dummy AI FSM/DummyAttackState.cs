using UnityEngine;


public class DummyAttackState : DummyDefaultState
{
   
    public override void EnterState(DummyStateManager dummy)
    {
        Debug.Log("dummy attacked!");

        //call the dummy jumpscare event
        TimelineManager.onPlayDummyJumpscare?.Invoke();

        //signal an event that lets the game know that the dummy attack
        DummyStateManager.dummyKilledPlayer = true;
      
        
  
    }

    public override void UpdateState(DummyStateManager dummy)
    {
        return;
    }
}
