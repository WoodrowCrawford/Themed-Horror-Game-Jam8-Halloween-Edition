using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SleepBehavior : MonoBehaviour
{
    [Header("Sleep HUD Image")]
    [SerializeField] private Image _eyesClosed;

    [Header("Sleep Values")]
    [SerializeField] private float _sleepSpeed;
    public float sleepMeter;
    public bool playerIsSleeping;
   
    private void Update()
    {
        Sleep();
        
    }


    public void Sleep()
    {
        if(PlayerInputBehavior.playerCanSleep && !PauseSystem.isPaused && !DialogueUIBehavior.IsOpen)
        {
            if (playerIsSleeping)
            {
                _eyesClosed.gameObject.SetActive(true);
                sleepMeter += Time.deltaTime * _sleepSpeed;
            }
            else if (!playerIsSleeping)
            {
                _eyesClosed.gameObject.SetActive(false);
            }
        }
        return;
    }


}
