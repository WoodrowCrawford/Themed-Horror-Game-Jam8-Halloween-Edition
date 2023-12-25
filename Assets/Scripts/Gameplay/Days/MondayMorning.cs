using UnityEngine;

public class MondayMorning : BaseDay
{
    public override void EnterState(DayManager day)
    {
        Debug.Log("monday morning enter state");
    }

    public override void UpdateState(DayManager day)
    {
        Debug.Log("monday morning update state");
    }

    public override void ExitState()
    {
        Debug.Log("monday morning exit state");
    }

    

    
}
