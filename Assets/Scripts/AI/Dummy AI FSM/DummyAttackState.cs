using UnityEngine;

public class DummyAttackState : DummyDefaultState
{
    

    public override void EnterState(DummyStateManager dummy)
    {
        GameManager.instance.SetGameOver();

        Debug.Log("dummy attacked!");
       
    }

    public override void UpdateState(DummyStateManager dummy)
    {
       
    }
}
