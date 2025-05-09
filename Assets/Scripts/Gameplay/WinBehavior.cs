using UnityEngine;
using UnityEngine.UI;

public class WinBehavior : MonoBehaviour
{
    public delegate void WinEventDelegate();

    public static event WinEventDelegate onWin;

    [SerializeField] private GameObject _winScreen; // The win screen

    [SerializeField] private Button _retryButton; // The retry button


    private void OnEnable()
    {
        SleepBehavior.onSleepMeterFilled += ShowWinScreen;
        _retryButton.onClick.AddListener(() => Debug.Log("The player wants to restart the game!"));
    }

    private void OnDisable()
    {
        SleepBehavior.onSleepMeterFilled -= ShowWinScreen;
        _retryButton.onClick.RemoveAllListeners();
    }



    public void ShowWinScreen()
    {
        _winScreen.SetActive(true);

        //call the onWin event
    }

    public void HideWinScreen()
    {
        _winScreen.SetActive(false);
    }


    

}
