using UnityEngine;


public class DummyAttackState : DummyDefaultState
{
   
    public override void EnterState(DummyStateManager dummy)
    {
        Debug.Log("dummy attacked!");

        //call the dummy jumpscare event
        TimelineManager.onPlayDummyJumpscare?.Invoke();
      
        
  
    }

    public override void UpdateState(DummyStateManager dummy)
    {
        return;
    }
}
