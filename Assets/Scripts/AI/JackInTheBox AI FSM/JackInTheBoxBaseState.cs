using UnityEngine;

public abstract class JackInTheBoxBaseState 
{
    public abstract void EnterState(JackInTheBoxStateManager jackInTheBox);

    public abstract void UpdateState(JackInTheBoxStateManager jackIntheBox);
}
