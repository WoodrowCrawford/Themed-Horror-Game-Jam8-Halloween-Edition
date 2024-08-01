using UnityEngine;
using UnityEngine.UI;

public class SleepBehavior : MonoBehaviour
{
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
        if(playerIsSleeping)
        {
            sleepMeter += Time.deltaTime * _sleepSpeed;
        }
        else
        {
            return;
        }
    }
  

   


    public void CloseEyes()
    {
        if(PauseSystem.isPaused || DialogueUIBehavior.IsOpen || !PlayerInputBehavior.playerCanSleep)
        {
            Debug.Log("the player can not close their eyes because the game is paused!");
        }
        else if(!PauseSystem.isPaused || !DialogueUIBehavior.IsOpen || PlayerInputBehavior.playerCanSleep)
        {
            _eyesClosed.gameObject.SetActive(true);
            playerIsSleeping = true;
            Debug.Log("The player is closing eyes!");
           
        }
    }

    public void OpenEyes()
    {
        //if the game is paused or the dialogue box is open or the player can not sleep...
        if(PauseSystem.isPaused || DialogueUIBehavior.IsOpen || !PlayerInputBehavior.playerCanSleep)
        {
            //open the eyes 
            _eyesClosed.gameObject.SetActive(false);
            playerIsSleeping = false;

            //debug
            Debug.Log("the players eyes are open because the game is paused or the dialogue box is open");
        }
        
        //else if the game is not paused or the dialogue box is not open or the player can sleep
        else if(!PauseSystem.isPaused || !DialogueUIBehavior.IsOpen || PlayerInputBehavior.playerCanSleep)
        {
            //open the eyes 
            _eyesClosed.gameObject.SetActive(false);
            playerIsSleeping = false;

            //debug
            Debug.Log("the player opened their eyes");
        }    
    }

}
