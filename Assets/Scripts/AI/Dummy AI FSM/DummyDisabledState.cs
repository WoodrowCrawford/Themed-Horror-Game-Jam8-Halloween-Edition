using UnityEngine;
public class DummyDisabledState : DummyDefaultState
{
    //This state is used for when the game is over
    //Stops all ai behavior

  
        
public override void EnterState(DummyStateManager dummy)
    {
        Debug.Log("Dummy is now in the disabled state!");

    }

   

    public override void UpdateState(DummyStateManager dummy)
    {
        return;
    }
}
