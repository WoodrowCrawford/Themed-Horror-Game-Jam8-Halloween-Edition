using UnityEngine;

public class TuesdayMorning : BaseDay
{

    public override void EnterState(DayManager day)
    {
        Debug.Log("Tuesday morning is now in the enter state");
        day.currentDay = DayManager.Days.TUESDAY_MORNING;
    }

    public override void UpdateState(DayManager day)
    {
        return;
    }

    public override void ExitState()
    {
        Debug.Log("Tuesday morning over. Stopping story...");
    }

   
}
