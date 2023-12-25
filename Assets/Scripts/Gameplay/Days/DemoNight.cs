using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DayManager;

public class DemoNight : BaseDay
{
    public override void EnterState(DayManager day)
    {
        Debug.Log("The demo is in the enter state");
        day.days = Days.DEMO;
        day.StartCoroutine(StartDemoNight());
    }


    public override void UpdateState(DayManager day)
    {
        Debug.Log("Demo update state");
    }

    public override void ExitState()
    {
        Debug.Log("Demo is over. Stopping script...");
        instance.StopCoroutine(StartDemoNight());
    }

    public IEnumerator StartDemoNight()
    {
        //This is the demo night
        //The player will be hit with the all the enemies and will have to sleep in order to win

        instance.days = Days.DEMO;
        GraphicsBehavior.instance.SetNightTime();

        //the player is able to sleep
        PlayerInputBehavior.playerCanSleep = true;

        instance.CallShowTodaysDate();


        //turn on the flashlight
        instance.flashlightBehavior.TurnOnFlashlight();



        //wait until the screen is finished loading
        yield return new WaitUntil(() => TodaysDateBehavior.instance.loadingScreenFinished);

        instance.GetInitializers();


        DialogueUIBehavior.instance.ShowDialogue(instance.demoIntroDialogue);

        yield return null;
    }
}
