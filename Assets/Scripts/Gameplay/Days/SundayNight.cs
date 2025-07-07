using System.Collections;
using UnityEngine;
using static DayManager;

public class SundayNight : BaseDay
{
    public override void EnterState(DayManager day)
    {
        Debug.Log("Sunday night enter state");
        day.currentDay = Days.SUNDAY_NIGHT;
        day.StartCoroutine(StartSundayNight());
    }


    public override void UpdateState(DayManager day)
    {
        return;
    }

    

    public override void ExitState()
    {
        Debug.Log("Finishing up sunday night state. Stopping story... ");
        Debug.Log("Sunday night exit state");
        instance.StopCoroutine(StartSundayNight());
    }


    public IEnumerator StartSundayNight()
    {

        //the player is able to sleep
        PlayerInputBehavior.playerCanSleep = true;

        instance.CallShowTodaysDate();


        //turn on the flashlight
        instance.flashlightBehavior.TurnOnFlashlight();

        //Sets current task to be sleep
        //task = SundayMorningTasks.SLEEP;

        //Set up variables
        instance.currentDay = Days.SUNDAY_NIGHT;
        GraphicsBehavior.instance.SetNightTime();



        //wait until the screen is finished loading
        yield return new WaitUntil(() => TodaysDateBehavior.instance.loadingScreenFinished);

        instance.GetInitializers();

        //wait a few seconds
        yield return new WaitForSeconds(0.1f);

        //telelports the dummy to go back to the original location
        instance.Dummy1.GetComponent<DummyStateManager>().gameObject.transform.position = instance.Dummy1.GetComponent<DummyStateManager>().OriginPos.position;

        //telelports the dummy to go back to the original location
        instance.Dummy2.GetComponent<DummyStateManager>().gameObject.transform.position = instance.Dummy2.GetComponent<DummyStateManager>().OriginPos.position;


        //Initializes the dummies
        DummyStateManager.InitializeDummyValues(instance.Dummy1, 1, 5, Random.Range(1, 11), Random.Range(12, 20), true, new Vector3(1f, 1f, 1f));
        DummyStateManager.InitializeDummyValues(instance.Dummy2, 1, 5, Random.Range(1, 11), Random.Range(12, 20), true, new Vector3(1f, 1f, 1f));



        //Initializes the clown
        ClownStateManager.InitializeClown(instance.Clown, false);


        //Initialize the ghoul
        GhoulStateManager.InitializeGhoulValues(instance.Ghoul, 0, 0, false);


        yield return new WaitForSeconds(0.5f);



        //Wait until the dialouge box is closed
        //yield return new WaitUntil(() => !DialogueUIBehavior.IsOpen);

        //wait
        // yield return new WaitForSeconds(3f);

        //show the wake up dialogue
        DialogueUIBehavior.instance.ShowDialogue(instance.sundayNightIntroDialouge);

        //Wait until the dialouge box is closed
        yield return new WaitUntil(() => !DialogueUIBehavior.IsOpen);

        //task = SundayMorningTasks.SLEEP;


        yield break;
    }

    
}
