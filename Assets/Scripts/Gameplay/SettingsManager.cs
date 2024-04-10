using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    //delegates
    public delegate void SettingsEvents();


    //events
    public static SettingsEvents onSettingsOpenedMainMenu;
    public static SettingsEvents onSettingsClosedMainMenu;

    public static SettingsEvents onSettingsOpenedMainGame;
    public static SettingsEvents onSettingsClosedMainGame;

    [Header("Game Objects")]
    [SerializeField] private GameObject _settingsBackground;
    [SerializeField] private GameObject _qualitySelector;

    [Header("Settings Buttons")]
    [SerializeField] private Button _backButton;

    [Header("Quality Buttons")]
    [SerializeField] private Button _lowQuality;
    [SerializeField] private Button _mediumQuality;
    [SerializeField] private Button _highQuality;
 
    

    private void OnEnable()
    {
        //quality buttons
        _lowQuality.onClick.AddListener(() => SetQualityLevel(0));
        _mediumQuality.onClick.AddListener(() => SetQualityLevel(1));
        _highQuality.onClick.AddListener(() => SetQualityLevel(2));

        _backButton.onClick.AddListener(() => CloseSettings());
    }


    private void OnDisable()
    {
        //quality buttons disable
        _lowQuality.onClick.RemoveListener(() => SetQualityLevel(0));
        _mediumQuality.onClick.RemoveListener(() => SetQualityLevel(1));
        _highQuality.onClick.RemoveListener(() => SetQualityLevel(2));

        _backButton.onClick.RemoveListener(() => CloseSettings());

    }



    public void SetQualityLevel(int level)
    {
        //sets the quality level to the given level 
        QualitySettings.SetQualityLevel(level);

        if (level == 0)
        {
            //show the quality selector
            _qualitySelector.gameObject.SetActive(true);

            //Set the quality selector to be equal to the quality selected
            _qualitySelector.transform.position = _lowQuality.gameObject.transform.position;
        }
        else if (level == 1)
        {
            //show the quality selector
            _qualitySelector.gameObject.SetActive(true);

            //Set the quality selector to be equal to the quality selected
            _qualitySelector.transform.position = _mediumQuality.gameObject.transform.position;
        }
        else if (level == 2)
        {
            //show the quality selector
            _qualitySelector.gameObject.SetActive(true);

            //Set the quality selector to be equal to the quality selected
            _qualitySelector.transform.position = _highQuality.gameObject.transform.position;
        }
    }


    public void CloseSettings()
    {
        //check if the scene is the main menu or main game
        if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainMenuScene"))
        {
            //close entire menu

            //Closes the settings background
            _settingsBackground.SetActive(false);
            onSettingsClosedMainMenu?.Invoke();
        
        }



        //else if main game then close settings screen and go back to the pause menu
        else
        {
            //Closes the settings background
            _settingsBackground.SetActive(false);
            onSettingsClosedMainGame?.Invoke();
       
        }

        

        

        
    }
}
