
using System.Collections;

public abstract class WindowBaseState
{
    public delegate void Exit();
    public static Exit onSwitchState;

    public abstract void EnterState(WindowStateManager window);

    public abstract void UpdateState(WindowStateManager window);

    public abstract void ExitState();
}
