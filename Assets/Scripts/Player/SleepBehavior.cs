using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class SleepBehavior : MonoBehaviour
{
    public delegate void SleepBehaviorDelegate();

    public static event SleepBehaviorDelegate onPlayerCloseEyes;  //what happens when the player starts to fall asleep
    public static event SleepBehaviorDelegate onPlayerOpenEyes;

    public static event SleepBehaviorDelegate onSleepMeterFilled; //what happens when the sleep meter is filled 
   

   
    [Header("Sleep Values")]
    [SerializeField] private float _sleepSpeed;
    public float sleepMeter;
    public static bool playerIsSleeping;

    [Header("Sleep UI Game Object")]
    [SerializeField] private GameObject _eyes;
    [SerializeField] private Animator _eyesAnimator;



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
            //play the eyes closed animation
            PlayEyesClosedAnimation();

            //call the player is sleeping event
            onPlayerCloseEyes?.Invoke();  

           

            //set the player is sleeping to true
            playerIsSleeping = true;         
        }
    }

    public void OpenEyes()
    {
        //if the game is paused or the dialogue box is open or the player can not sleep...
        if(PauseSystem.isPaused || DialogueUIBehavior.IsOpen || !PlayerInputBehavior.playerCanSleep)
        {
           Debug.Log("The player can not sleep at the moment");
           return;

        }
        
        //else if the game is not paused or the dialogue box is not open or the player can sleep
        else if(!PauseSystem.isPaused || !DialogueUIBehavior.IsOpen || PlayerInputBehavior.playerCanSleep)
        {
            //play the eyes opened animation
            PlayEyesOpenedAnimation();

            //call the player opened eyes event
            onPlayerOpenEyes?.Invoke();

            //set player is sleeping to false
            playerIsSleeping = false;
        }    
    }


    public void PlayEyesClosedAnimation()
    {

        //call the trigger event
        _eyesAnimator.SetTrigger("ClosedEyes");
    }

    public void PlayEyesOpenedAnimation()
    {
        //call the trigger event
        _eyesAnimator.SetTrigger("OpenedEyes");
    }

}
