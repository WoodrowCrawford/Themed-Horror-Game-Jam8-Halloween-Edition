using UnityEngine;

public class DummyAttackState : DummyDefaultState
{
    public override void EnterState(DummyStateManager dummy)
    {
        GameOverBehavior.SetGameOver(true);
    }

    public override void UpdateState(DummyStateManager dummy)
    {
       
    }
}
