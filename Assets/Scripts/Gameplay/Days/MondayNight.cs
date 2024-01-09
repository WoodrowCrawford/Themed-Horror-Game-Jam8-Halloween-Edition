using UnityEngine;

public class MondayNight : BaseDay
{
    public override void EnterState(DayManager day)
    {
        Debug.Log("monday night enter state");
    }

    public override void ExitState()
    {
        Debug.Log("monday night exit state");
    }

 

    public override void UpdateState(DayManager day)
    {
        Debug.Log("monday night update state");
    }
}
