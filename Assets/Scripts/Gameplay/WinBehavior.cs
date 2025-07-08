using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameManager;


public class WinBehavior : MonoBehaviour
{
    public delegate void WinEventDelegate();

    public static event WinEventDelegate onWin;

    [SerializeField] private GameObject _winScreen; // The win screen

    [Header("Button References")]
    [SerializeField] private Button _retryButton;
    [SerializeField] private Button _mainMenuButton;




    private void OnEnable()
    {
        SleepBehavior.onSleepMeterFilled += ShowWinScreen;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;

    }

    private void OnDisable()
    {
        SleepBehavior.onSleepMeterFilled -= ShowWinScreen;
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;

    }


    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
       

        //if the scene is the main menu scene
        if (scene == SceneManager.GetSceneByBuildIndex(0))
        {
            _retryButton = null;
            _mainMenuButton = null;
        }

        //if the scene is the bedroom scene
        else if (scene == SceneManager.GetSceneByBuildIndex(1))
        {
            //get a list of all the buttons in the scene 
            Button[] allButtons = Resources.FindObjectsOfTypeAll<Button>();

            foreach (Button button in allButtons)
            {
                //if the button is the retry button
                if (button.name == "RetryButton")
                {
                    _retryButton = button;
                    _retryButton.onClick.AddListener(() => SceneManager.LoadScene(1)); // Load the bedroom scene
                }
                //if the button is the main menu button
                else if (button.name == "MainMenuButton")
                {
                    _mainMenuButton = button;
                    _mainMenuButton.onClick.AddListener(() => SceneManager.LoadScene(0)); // Load the main menu scene
                }
            }
        }
    }


    public void OnSceneUnloaded(Scene scene)
    {
      
        //the the currnt scene was the main menu and it changes...
        if (scene == SceneManager.GetSceneByBuildIndex(0))
        {
           _mainMenuButton = null;
            _retryButton = null;
        }

        //if the current scene was the bedroom scene and it changes...
        else if (scene == SceneManager.GetSceneByBuildIndex(1))
        {
            _retryButton = null;
            _retryButton = null;
        }


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
