using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance;

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

    [Header("Brightness Values")]
    [SerializeField] private Slider _brightnessSlider;
    [SerializeField] private PostProcessProfile _postProcessProfile;
    [SerializeField] private PostProcessLayer _layer;
    AutoExposure exposure;
    
 
    



    private void OnEnable()
    {
        //add listners for the quality buttons
        _lowQuality.onClick.AddListener(() => SetQualityLevel(0));
        _mediumQuality.onClick.AddListener(() => SetQualityLevel(1));
        _highQuality.onClick.AddListener(() => SetQualityLevel(2));

        //add listeners for the back button
        _backButton.onClick.AddListener(() => CloseSettings());

        //GameManager.onGameStarted += FindSettingsObject;
    }

   
    private void OnDisable()
    {
        //remove listeners for the quality buttons
        _lowQuality.onClick.RemoveListener(() => SetQualityLevel(0));
        _mediumQuality.onClick.RemoveListener(() => SetQualityLevel(1));
        _highQuality.onClick.RemoveListener(() => SetQualityLevel(2));

        //remove listeners for the back buttons
        _backButton.onClick.RemoveListener(() => CloseSettings());

        //GameManager.onGameStarted -= FindSettingsObject;

    }



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


    private void Start()
    {
        //get the quality level on startup
        GetQualityLevel();

        _postProcessProfile.TryGetSettings(out exposure);
        AdjustBrightness(_brightnessSlider.value);
    }


    

    public void AdjustBrightness(float value)
    {
        //if the value does not equal 0
        if (value != 0)
        {
            //set the value of the brightness to the exposure key value
            exposure.keyValue.value = value;
        }
        else
        {
            //set the value of the brightness to .05;
            exposure.keyValue.value = .05f;
            
        }
    }

    public void GetQualityLevel()
    {
        int qualityLevel = QualitySettings.GetQualityLevel();

        if( qualityLevel == 0)
        {
            //show the quality selector
            _qualitySelector.gameObject.SetActive(true);

            //Set the quality selector to be equal to the quality selected
            _qualitySelector.transform.position = _lowQuality.gameObject.transform.position;
        }
        else if( qualityLevel == 1)
        {
            //show the quality selector
            _qualitySelector.gameObject.SetActive(true);

            //Set the quality selector to be equal to the quality selected
            _qualitySelector.transform.position = _mediumQuality.gameObject.transform.position;
        }
        else if( qualityLevel == 2)
        {
            //show the quality selector
            _qualitySelector.gameObject.SetActive(true);

            //Set the quality selector to be equal to the quality selected
            _qualitySelector.transform.position = _highQuality.gameObject.transform.position;
        }
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

    public void FindSettingsObject()
    {
        _settingsBackground = GameObject.FindGameObjectWithTag("Settings");

        Debug.Log("Find the settings and set it here~");
    }
}
