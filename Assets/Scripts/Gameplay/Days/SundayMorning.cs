using System.Collections;
using UnityEngine;
using static DayManager;

public class SundayMorning : BaseDay
{
    

    public enum SundayMorningTasks
    {
        NONE,
        LOOK_AROUND,
        CLEAN_UP,
        SAY_GOODNIGHT_TO_TOYS,
        GO_TO_BED,
        SLEEP
    }


    //checks to see if the player has interacted with all the objects in the room
    private bool _playerInteractedWithAllTheObjects { get { return BasketBallInteractable.IsInteracted && BeanbagInteractable.IsInteracted && JackInTheBoxBehavior.IsInteracted && ClownStateManager.IsInteracted && DummyStateManager.IsInteracted; } }

    private bool _saidGoodnightToAllToys { get { return RexDogInteractable.playerSaidGoodnight && TeadybearInteractable.playerSaidGoodnight; } }
    public static bool playerPutAllTheToysInTheToyBox { get { return BasketBallInteractable.IsInTheToyBox; } }

    public static bool startGoToBedPhase = false;




    public override void EnterState(DayManager day)
    {
        day.StartCoroutine(StartSundayMorning());
    }


    public override void UpdateState(DayManager day)
    {
        Debug.Log("sunday morning update state");
    }

    


    public override void ExitState()
    {
        Debug.Log("Finishing up sunday morning state. Stopping story... ");
        instance.StopCoroutine(StartSundayMorning());
    }

    public IEnumerator StartSundayMorning()
    {


        //the player is not able to sleep
        PlayerInputBehavior.playerCanSleep = false;

        //find the sun
        instance.graphicsBehavior.Sun = GameObject.FindGameObjectWithTag("Sun");

        instance.CallShowTodaysDate();


        //turn off the flashlight
        instance.flashlightBehavior.TurnOffFlashlight();

        //Sets current task to be nothing on start up
        instance.currentSundayMorningTask = SundayMorningTasks.NONE;

        //Set up variables
        instance.currentDay = Days.SUNDAY_MORNING;
        GraphicsBehavior.instance.SetDayTime();



        //wait until the screen is finished loading
        yield return new WaitUntil(() => TodaysDateBehavior.instance.loadingScreenFinished);


        instance.GetInitializers();




        //Initializes the dummies
        DummyStateManager.InitializeDummyValues(instance.Dummy1, 0, 0, 0, 0, false, new Vector3(0.5f, 0.5f, 0.5f));
        DummyStateManager.InitializeDummyValues(instance.Dummy2, 0, 0, 0, 0, false, new Vector3(0.5f, 0.5f, 0.5f));



        //Initializes the clown
        ClownStateManager.InitializeClown(instance.Clown, false);


        //Initialize the ghoul
        GhoulStateManager.InitializeGhoulValues(instance.Ghoul, 0, 0, false);







        //Wait until the dialouge box is closed
        //yield return new WaitUntil(() => !DialogueUIBehavior.IsOpen);

        //wait
        // yield return new WaitForSeconds(3f);

        //show the wake up dialogue
        SundayMorningDialogueManager.instance.PlayDialogue(SundayMorningDialogueManager.instance.wakeUpDialouge);
   

        //Wait until the dialouge box is closed
        yield return new WaitUntil(() => !DialogueUIBehavior.IsOpen);

        /////////The player is then tasked with looking around
        /////////They will look around and interact with objects.
        instance.currentSundayMorningTask = SundayMorningTasks.LOOK_AROUND;
        


        //Check to make sure that the player examined everything here
        yield return new WaitUntil(() => _playerInteractedWithAllTheObjects && !DialogueUIBehavior.IsOpen);

        //After examining everything, the player's parents will tell them that they need to clean up the room (start a dialogue here saying that)
        SundayMorningDialogueManager.instance.PlayDialogue(SundayMorningDialogueManager.instance.cleanUpDialogue);

        //The player will then have the task of picking up items and putting them away.
        //(create a task for the player to clean up. items should do different things when interacted now, since they need to be picked up)
        instance.currentSundayMorningTask = SundayMorningTasks.CLEAN_UP;

        //When the player tries to put away the dummies, whenever the player is not looking at the dummies, move them back to the spot they began.
        //Do this a few times. (This will be done in the toybox trigger behavior)
        //Afterwards, the dummies will stay in the spot that the player moved them to, and the player will then get exhaused and try to go to sleep, starting the next phase.


        //check to see if the player can start to go to bed for the day
        yield return new WaitUntil(() => startGoToBedPhase);

        //waits a few seconds
        yield return new WaitForSeconds(2f);

        //plays the dialogue 
        SundayMorningDialogueManager.instance.PlayDialogue(SundayMorningDialogueManager.instance.gettingSleepyDialogue);

      

        //wait until the dialogue box is closed
        yield return new WaitUntil(() => !DialogueUIBehavior.IsOpen);

        //set the task to be say goodnight to the toys
        instance.currentSundayMorningTask = SundayMorningTasks.SAY_GOODNIGHT_TO_TOYS;

        //wait until the player said goodnight to all the toys
        yield return new WaitUntil(() => _saidGoodnightToAllToys);

        //wait a few seconds0
        yield return new WaitForSeconds(2f);

        //show the dialogue
        SundayMorningDialogueManager.instance.PlayDialogue(SundayMorningDialogueManager.instance.goToBedDialogue);
   


        //sets the task to be "go to bed"
        instance.currentSundayMorningTask = SundayMorningTasks.GO_TO_BED;

        //wait until the player is in the bed
        yield return new WaitUntil(() => PlayerInputBehavior.inBed);

        yield return new WaitForSeconds(1f);




        //the player goes to sleep, then the next phase happens

        instance.currentDay = Days.SUNDAY_NIGHT;


        // StartCoroutine(StartSundayNight());

        //switch to the next state, day





        yield return null;
    }

  
}
