using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIBehavior : MonoBehaviour
{
    [Header("Game Objects")]
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

        //Main menu
        _playButton.onClick.AddListener(() => GameManager.ChangeScene("BedroomScene"));
        _settingsButton.onClick.AddListener(() => OpenSettings());

        
    }

    private void OnDisable()
    {
        //events on disable
        SettingsManager.onSettingsOpenedMainMenu -= HideMainMenuButtons;
        SettingsManager.onSettingsClosedMainMenu -= ShowMainMenuButtons;


        //Main menu on disable
        _playButton.onClick.RemoveListener(() => GameManager.ChangeScene("BedroomScene"));
        _settingsButton.onClick.RemoveListener(() => OpenSettings());
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
}
