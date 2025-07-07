using UnityEngine;
using UnityEngine.UI;


public class WinBehavior : MonoBehaviour
{
    public delegate void WinEventDelegate();

    public static event WinEventDelegate onWin;

    [SerializeField] private GameObject _winScreen; // The win screen



    private void OnEnable()
    {
        SleepBehavior.onSleepMeterFilled += ShowWinScreen;
       
    }

    private void OnDisable()
    {
        SleepBehavior.onSleepMeterFilled -= ShowWinScreen;
       
    }



    public void ShowWinScreen()
    {
        //set the win screen to be true
        _winScreen.SetActive(true);

        //show the mouse cursor
        Cursor.visible = true;

        //call the onWin event
        onWin?.Invoke();
    }



    public void HideWinScreen()
    {
        _winScreen.SetActive(false);
    }
}
