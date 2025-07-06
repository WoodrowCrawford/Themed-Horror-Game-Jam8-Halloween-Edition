using System.Collections;
using UnityEngine;
using static DayManager;

public class DemoNight : BaseDay
{
  
    public enum DemoNightTasks
    { 
        NONE,
        EXAMINE_ROOM,
        SLEEP
    }

  

    public override void EnterState(DayManager day)
    {
        Debug.Log("The demo is in the enter state");
        day.currentDay = Days.DEMO;
        day.StartCoroutine(StartDemoNight());
    
        
    }


    public override void UpdateState(DayManager day)
    {
        return;
    }

    public override void ExitState()
    {
        Debug.Log("Demo is over. Stopping script...");
        instance.StopCoroutine(StartDemoNight());
    }


    public void ExamnineRoomTask()
    {
        Debug.Log("Examine room event is running");

        //tell the flashlight that it can not deplete the battery
        FlashlightBehavior.flashlightCanDeplete = false;

        //the player is not able to sleep
        PlayerInputBehavior.playerCanSleep = false;
    }

    public void SleepTask()
    {
        //the player is able to sleep
        PlayerInputBehavior.playerCanSleep = true;

        //Make enemies active here
        GhoulStateManager.InitializeGhoulValues(instance.Ghoul, 15, 35, false);
        WindowStateManager.InitializeWindowValues(instance.Window, 9, 20, 6, 10, false);
        JackInTheBoxStateManager.InitializeJackInTheBox(instance.JackInTheBox, 2f, 20f, true);
        ClownStateManager.InitializeClown(instance.Clown, true);  

        DummyStateManager.InitializeDummyValues(instance.Dummy1, 3f, 6f, 10f, 20f, false, new Vector3(0.5f, 0.5f, 0.5f));
        DummyStateManager.InitializeDummyValues(instance.Dummy2, 3f, 6f, 10f, 20f, false, new Vector3(0.5f, 0.5f, 0.5f));



        Debug.Log("sleep task event is running");

        //Tell the flashlight that it can start delpleting the battery
        FlashlightBehavior.flashlightCanDeplete = true;
    }


    public IEnumerator StartDemoNight()
    {
        //This is the demo night
        //The player will be hit with the all the enemies and will have to sleep in order to win

      

        //Sets to be false
        FlashlightBehavior.flashlightCanDeplete = false;



        instance.currentDay = Days.DEMO;
       

       

        instance.CallShowTodaysDate();


        //turn on the flashlight
        instance.flashlightBehavior.TurnOnFlashlight();



        //wait until the screen is finished loading
        yield return new WaitUntil(() => TodaysDateBehavior.instance.loadingScreenFinished);

        //gets the variables needed to start the game
        instance.GetInitializers();

        OnNightTime?.Invoke();


        //play the demo night intro dialogue
        instance.demoDialogueManager.PlayDialogue(instance.demoDialogueManager.demoNightIntroDialogue);
        

        //wait until the dialogue box is closed
        yield return new WaitUntil(() => !DialogueUIBehavior.IsOpen);
        
        //if the task is to examine room...
        if(instance.currentDemoNightTask == DemoNightTasks.EXAMINE_ROOM)
        {
            //let the player look around until they want to get back into the bed to start the night
            ExamnineRoomTask();
        

            //wait until the task is to fall asleep
            yield return new WaitUntil(() => instance.currentDemoNightTask == DemoNightTasks.SLEEP);
            SleepTask();
        }
        else if(instance.currentDemoNightTask == DemoNightTasks.SLEEP)
        {
            SleepTask();
        }

        
        
        yield return null;
    }
}
