using UnityEngine;
using UnityEngine.UI;

public class SleepBehavior : MonoBehaviour
{
    public delegate void SleepBehaviorDelegate();

    public static event SleepBehaviorDelegate onPlayerCloseEyes;  //what happens when the player starts to fall asleep
    public static event SleepBehaviorDelegate onPlayerOpenEyes;

    public static event SleepBehaviorDelegate onSleepMeterFilled; //what happens when the sleep meter is filled 
   

    [Header("Sleep HUD Image")]
    [SerializeField] private Image _eyesClosed;

    [Header("Sleep Values")]
    [SerializeField] private float _sleepSpeed;
    public float sleepMeter;
    public static bool playerIsSleeping;



    private void OnEnable()
    {
        PlayerInputBehavior.onSleepButtonReleased += OpenEyes;
       PlayerInputBehavior.onSleepButtonPressed += CloseEyes;

        
    }

    private void OnDisable()
    {
       PlayerInputBehavior.onSleepButtonReleased -= OpenEyes;
       PlayerInputBehavior.onSleepButtonPressed -= CloseEyes;

    }


    private void Update()
    {
        UpdateSleepMeter();
    }


    public void UpdateSleepMeter()
    {
        //if the player is sleeping...
        if(playerIsSleeping)
        {
            sleepMeter += Time.deltaTime * _sleepSpeed;

            //if the meter is filled all the way...
            if (sleepMeter >= 100)
            {
                //invoke the sleep meter filled event
                onSleepMeterFilled?.Invoke();
            }
        }
        else
        {
            return;
        }
    }
  

   


    public void CloseEyes()
    {
        //if the game is paused or the dialogue box is open or the player is not able to sleep...
        if(PauseSystem.isPaused || DialogueUIBehavior.IsOpen || !PlayerInputBehavior.playerCanSleep)
        {
            //return 
            Debug.Log("the player can not close their eyes because the game is paused!");
            return;
            
        }

        //else if the game is not paused or the dialogue box is not open or the player can sleep...
        else if(!PauseSystem.isPaused || !DialogueUIBehavior.IsOpen || PlayerInputBehavior.playerCanSleep)
        {

            //call the player is sleeping event
            onPlayerCloseEyes?.Invoke();



            //OLD
            //_eyesClosed.gameObject.SetActive(true);
            //playerIsSleeping = true;
            //Debug.Log("The player is closing eyes!");
           
        }
    }

    public void OpenEyes()
    {
        //if the game is paused or the dialogue box is open or the player can not sleep...
        if(PauseSystem.isPaused || DialogueUIBehavior.IsOpen || !PlayerInputBehavior.playerCanSleep)
        {
           ///FIX TO MAKE WORK!!!!!!!


            //OLD
            //_eyesClosed.gameObject.SetActive(false);
            //playerIsSleeping = false;

        }
        
        //else if the game is not paused or the dialogue box is not open or the player can sleep
        else if(!PauseSystem.isPaused || !DialogueUIBehavior.IsOpen || PlayerInputBehavior.playerCanSleep)
        {
            onPlayerOpenEyes?.Invoke();

            ////open the eyes 
            //_eyesClosed.gameObject.SetActive(false);
            //playerIsSleeping = false;

            //debug
            Debug.Log("the player opened their eyes");
        }    
    }

}
