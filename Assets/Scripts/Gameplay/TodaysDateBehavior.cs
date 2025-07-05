using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TodaysDateBehavior : MonoBehaviour
{
    public static TodaysDateBehavior instance;

    public delegate void TodaysDateEventHandler();

    public static event TodaysDateEventHandler onDateStartedShowning;
    public static event TodaysDateEventHandler onDateFinishedShowing;


    [SerializeField] private GameObject _todaysDateUI;  //UI background for the date
    [SerializeField] private TMP_Text _todaysDateText;  //text for the todays date

   
    public bool loadingScreenFinished = false;


    public TMP_Text TodaysDateText { get { return _todaysDateText;} set {  _todaysDateText = value; } }

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


    public void SetTodaysDate(string TodaysDate)
    {
        _todaysDateText.text = TodaysDate;
    }


    public IEnumerator ShowTodaysDate()
    {
        //set to be false on startup
        loadingScreenFinished = false;


       

        onDateStartedShowning?.Invoke();

        _todaysDateUI.SetActive(true);

        //waits a few seconds
        yield return new WaitForSeconds(2f);

        //sets the loading screen to be false
        _todaysDateUI.SetActive(false);


        //enables the player to pause
        PlayerInputBehavior.playerCanPause = true;

        onDateFinishedShowing?.Invoke();

        //sets to be true
        loadingScreenFinished = true;

    }
}
