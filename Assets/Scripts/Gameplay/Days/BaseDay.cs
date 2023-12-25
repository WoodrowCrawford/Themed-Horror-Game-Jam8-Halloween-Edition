
public abstract class BaseDay
{
    public delegate void Exit();
    public static Exit onSwitchState;


    public abstract void EnterState(DayManager day);       //Enter state

    public abstract void UpdateState(DayManager day);      //Update state

    public abstract void ExitState();                     //Exit state used when the current state has been switched to a new state

}
