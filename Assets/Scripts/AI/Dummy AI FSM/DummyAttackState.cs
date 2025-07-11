using UnityEngine;


public class DummyAttackState : DummyDefaultState
{
   
    public override void EnterState(DummyStateManager dummy)
    {
        //first check if the player can be attacked
        if(PlayerInputBehavior.playerCanGetCaught)
        {
            Debug.Log("dummy attacked!");

           

            //find which dummy object this is
            if(dummy.dummyThisBelongsTo.gameObject.name == "Dummy1")
            {
                
                //call the dummy jumpscare 1 event
                TimelineManager.onPlayDummy1Jumpscare?.Invoke();

                //Tell this dummy to stop moving
                dummy.dummyThisBelongsTo.GetComponent<DummyStateManager>().Agent.isStopped = true;
            }

            else if(dummy.dummyThisBelongsTo.gameObject.name == "Dummy2")
            {
                //call the dummy jumpscare event
                TimelineManager.onPlayDummy2Jumpscare?.Invoke();

                //Tell this dummy to stop moving
                dummy.dummyThisBelongsTo.GetComponent<DummyStateManager>().Agent.isStopped = true;
            }

            //signal an event that lets the game know that the dummy attacked
            DummyStateManager.dummyKilledPlayer = true;

            
        }
  
    }

    public override void UpdateState(DummyStateManager dummy)
    {
        return;
    }
}
