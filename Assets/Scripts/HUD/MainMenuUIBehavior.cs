using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIBehavior : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] private GameObject _settingsUI;
    [SerializeField] private GameObject _settingsBackground;


    [Header("Main Menu Buttons")]
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _settingsButton;

    


    [SerializeField]
    private void OnEnable()
    {
        //events
        SettingsManager.onSettingsOpenedMainMenu += HideMainMenuButtons;
        SettingsManager.onSettingsClosedMainMenu += ShowMainMenuButtons;

        GameManager.onStopStory += FindSettings;

        //Main menu
        _playButton.onClick.AddListener(() => GameManager.ChangeScene("BedroomScene"));
        _settingsButton.onClick.AddListener(() => OpenSettings());
      

        
    }

    private void OnDisable()
    {
        //events on disable
        SettingsManager.onSettingsOpenedMainMenu -= HideMainMenuButtons;
        SettingsManager.onSettingsClosedMainMenu -= ShowMainMenuButtons;

        GameManager.onStopStory -= FindSettings;

        //Main menu on disable
        _playButton.onClick.RemoveListener(() => GameManager.ChangeScene("BedroomScene"));
        _settingsButton.onClick.RemoveListener(() => OpenSettings());
        
    }


  

    private void Awake()
    {
        //Finds the settings on awake
        FindSettings();
    }




    public void ShowMainMenuButtons()
    {
        //shows the main menu buttons
        _playButton?.gameObject.SetActive(true);
        _settingsButton?.gameObject.SetActive(true);
    }

    public void HideMainMenuButtons()
    {
        //hides the main menu buttons
        _playButton?.gameObject.SetActive(false);
        _settingsButton?.gameObject.SetActive(false);
    }


    public void OpenSettings()
    {
        SettingsManager.onSettingsOpenedMainMenu?.Invoke();

        //Opens the settings background
        _settingsBackground?.SetActive(true);

        //Hides the play and settings butotns
        _playButton.gameObject.SetActive(false);
        _settingsButton.gameObject.SetActive(false);
    }

    public void FindSettings()
    {
        //find the settings UI canvas
        _settingsUI = GameObject.FindGameObjectWithTag("SettingsUI");


        //Find the settings game object in the canvas
        _settingsBackground = _settingsUI.transform.Find("Settings").gameObject;
    }

   
}
