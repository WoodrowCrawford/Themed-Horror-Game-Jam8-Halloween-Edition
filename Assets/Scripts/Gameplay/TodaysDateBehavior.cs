using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TodaysDateBehavior : MonoBehaviour
{
   
    public GameObject TodaysDateUI;  //UI background for the date

    public TMP_Text TodaysDateText;

    public static TodaysDateBehavior instance;

    public bool _loadingScreenFinished = false;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

   

    

    public IEnumerator ShowTodaysDate()
    {
        //set to be false on startup
        _loadingScreenFinished = false;


        //disable pausing while screen is showing
        PlayerInputBehavior.playerCanPause = false;

        //disable player movement
        PlayerInputBehavior.playerCanMove = false;
        PlayerInputBehavior.playerCanInteract = false;
        PlayerInputBehavior.playerCanGetOutOfBed = false;
        PlayerInputBehavior.playerCanToggleUnderBed = false;

        TodaysDateUI.SetActive(true);

        //waits a few seconds
        yield return new WaitForSeconds(2f);

        //sets the loading screen to be false
        TodaysDateUI.SetActive(false);
        

        //enable player movement stuff
        PlayerInputBehavior.playerCanMove = true;
        PlayerInputBehavior.playerCanInteract = true;
        PlayerInputBehavior.playerCanGetOutOfBed = true;
        PlayerInputBehavior.playerCanToggleUnderBed = true;

        //enables the player to pause
        PlayerInputBehavior.playerCanPause = true;

        //sets to be true
        _loadingScreenFinished = true;

    }
}
