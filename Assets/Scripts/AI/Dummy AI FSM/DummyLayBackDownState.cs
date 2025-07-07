using UnityEngine;

public class DummyLayBackDownState : DummyDefaultState
{
    public override void EnterState(DummyStateManager dummy)
    {
        //Test
        Debug.Log("Dummy is in the lay back down state");

        //Makes the dummy go back to the lay down animation
        dummy.dummyThisBelongsTo.GetComponent<DummyStateManager>().DummyLayDownAnimation();

        //Start the phase again
        dummy.dummyThisBelongsTo.GetComponent<DummyStateManager>().SwitchState(dummy.layingDownState);
       
    }

    public override void UpdateState(DummyStateManager dummy)
    {
        return;
    }

   
}
