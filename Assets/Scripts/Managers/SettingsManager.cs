using System;
using UnityEditor.Media;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance;



    [Header("Quality Buttons")]
    [SerializeField] private Button _lowQualityButton;
    [SerializeField] private Button _mediumQualityButton;
    [SerializeField] private Button _highQualityButton;

    [Header("Post Processing Buttons")]
    [SerializeField] private Button _motionBlurOffButton;
    [SerializeField] private Button _motionBlurLowButton;
    [SerializeField] private Button _motionBlurMediumButton;
    [SerializeField] private Button _motionBlurHighButton;

    [Header("Sensitivity Slider")]
    [SerializeField] private Slider _sensitivitySlider;

    [Header("Brightness Slider")]
    [SerializeField] private Slider _brightnessSlider;

    [Header("Sound Settings")]
    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;


    //settings getters

   




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



    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }





    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        //if the scene is the main menu
        if (scene == SceneManager.GetSceneByBuildIndex(0))
        {
            //get a list of all the buttons in the scene 
            Button[] allGameObjects = Resources.FindObjectsOfTypeAll<Button>();

            //get a list of all the sliders in the scene 
            Slider[] allSliders = Resources.FindObjectsOfTypeAll<Slider>();

            foreach (Slider slider in allSliders)
            {
                //if the slider is the sensitivity slider
                if (slider.name == "SensitivitySlider")
                {
                    //set the sensitivity slider to be the slider
                    _sensitivitySlider = slider;

                    //set the slider value to the current sensitivity
                    _sensitivitySlider.value = PlayerInputBehavior.sensitivity;

                    //add a listener to the slider
                    _sensitivitySlider.onValueChanged.AddListener(AdjustSensitivity);
                }

                //if the slider is the brightness slider
                if (slider.name == "BrightnessSlider")
                {
                    //set the brightness slider to be the slider
                    _brightnessSlider = slider;

                    //set the slider value to the current brightness
                    _brightnessSlider.value = GetBrightnessLevel();

                    //add a listener to the slider
                    _brightnessSlider.onValueChanged.AddListener(AdjustBrightness);
                }

                //if the slider is the master volume slider
                if (slider.name == "MasterVolumeSlider")
                {
                    //set the master volume slider to be the slider
                    _masterVolumeSlider = slider;

                    //set the slider value to the current master volume
                    _masterVolumeSlider.value = SoundMixerManager.instance.GetMasterVolume();

                    //add a listener to the slider
                    _masterVolumeSlider.onValueChanged.AddListener(SoundMixerManager.instance.SetMasterVolume);
                }

                //if the slider is the music volume slider
                if (slider.name == "MusicVolumeSlider")
                {
                    //set the music volume slider to be the slider
                    _musicVolumeSlider = slider;

                    //get the current music volume
                    _musicVolumeSlider.value = SoundMixerManager.instance.GetMusicVolume();

                    //add a listener to the slider
                    _musicVolumeSlider.onValueChanged.AddListener(SoundMixerManager.instance.SetMusicVolume);
                }

                //if the slider is the sound fx volume slider
                if (slider.name == "SoundFXVolumeSlider")
                {
                    //set the sound fx volume slider to be the slider
                    _sfxVolumeSlider = slider;

                    //get the current sound fx volume
                    _sfxVolumeSlider.value = SoundMixerManager.instance.GetSoundFXVolume();

                    //add a listener to the slider
                    _sfxVolumeSlider.onValueChanged.AddListener(SoundMixerManager.instance.SetSoundFXVolume);
                }
            }


            //for each button in the scene
            foreach (Button button in allGameObjects)
            {
                if (button.name == "LowQualityButton")
                {
                    //set the low quality button to be the button
                    _lowQualityButton = button;

                    //add a listener to the button
                    _lowQualityButton.onClick.AddListener(() => SetQualityLevel(0));
                }

                if (button.name == "MediumQualityButton")
                {
                    //set the medium quality button to be the button
                    _mediumQualityButton = button;

                    //add a listener to the button
                    _mediumQualityButton.onClick.AddListener(() => SetQualityLevel(1));
                }

                if (button.name == "HighQualityButton")
                {
                    //set the high quality button to be the button
                    _highQualityButton = button;

                    //add a listener to the button
                    _highQualityButton.onClick.AddListener(() => SetQualityLevel(2));
                }

                //if the button is the no motion blur button
                if (button.name == "NoMotionBlurButton")
                {
                    //set the motion blur off button to be the button
                    _motionBlurOffButton = button;

                    //add a listener to the button
                    _motionBlurOffButton.onClick.AddListener(() => SetMotionBlur(0));
                }

                //if the button is the low
                if (button.name == "LowMotionBlurButton")
                {
                    //set the motion blur low button to be the button
                    _motionBlurLowButton = button;

                    //add a listener to the button
                    _motionBlurLowButton.onClick.AddListener(() => SetMotionBlur(1));
                }

                if (button.name == "MediumMotionBlurButton")
                {
                    //set the motion blur medium button to be the button
                    _motionBlurMediumButton = button;

                    //add a listener to the button
                    _motionBlurMediumButton.onClick.AddListener(() => SetMotionBlur(2));
                }

                if (button.name == "HighMotionBlurButton")
                {
                    //set the motion blur high button to be the button
                    _motionBlurHighButton = button;

                    //add a listener to the button
                    _motionBlurHighButton.onClick.AddListener(() => SetMotionBlur(3));
                }
            }

        }

        //else if the scene is the bedroom scene
        else if (scene == SceneManager.GetSceneByBuildIndex(1))
        {
            //get a list of all the buttons in the scene 
            Button[] allGameObjects = Resources.FindObjectsOfTypeAll<Button>();

            //get a list of all the sliders in the scene
            Slider[] allSliders = Resources.FindObjectsOfTypeAll<Slider>();


            foreach (Slider slider in allSliders)
            {
                //if the slider is the sensitivity slider
                if (slider.name == "SensitivitySlider")
                {
                    //set the sensitivity slider to be the slider
                    _sensitivitySlider = slider;

                    //set the slider value to the current sensitivity
                    _sensitivitySlider.value = PlayerInputBehavior.sensitivity;

                    //add a listener to the slider
                    _sensitivitySlider.onValueChanged.AddListener(AdjustSensitivity);
                }

                //if the slider is the brightness slider
                if (slider.name == "BrightnessSlider")
                {
                    //set the brightness slider to be the slider
                    _brightnessSlider = slider;

                    //set the slider value to the current brightness
                    _brightnessSlider.value = GetBrightnessLevel();

                    //add a listener to the slider
                    _brightnessSlider.onValueChanged.AddListener(AdjustBrightness);
                }

                //if the slider is the master volume slider
                if (slider.name == "MasterVolumeSlider")
                {
                    //set the master volume slider to be the slider
                    _masterVolumeSlider = slider;

                    //set the slider value to the current master volume
                    _masterVolumeSlider.value = SoundMixerManager.instance.GetMasterVolume();

                    //add a listener to the slider
                    _masterVolumeSlider.onValueChanged.AddListener(SoundMixerManager.instance.SetMasterVolume);
                }

                //if the slider is the music volume slider
                if (slider.name == "MusicVolumeSlider")
                {
                    //set the music volume slider to be the slider
                    _musicVolumeSlider = slider;

                    //get the current music volume
                    _musicVolumeSlider.value = SoundMixerManager.instance.GetMusicVolume();

                    //add a listener to the slider
                    _musicVolumeSlider.onValueChanged.AddListener(SoundMixerManager.instance.SetMusicVolume);
                }

                //if the slider is the sound fx volume slider
                if (slider.name == "SoundFXVolumeSlider")
                {
                    //set the sound fx volume slider to be the slider
                    _sfxVolumeSlider = slider;

                    //get the current sound fx volume
                    _sfxVolumeSlider.value = SoundMixerManager.instance.GetSoundFXVolume();

                    //add a listener to the slider
                    _sfxVolumeSlider.onValueChanged.AddListener(SoundMixerManager.instance.SetSoundFXVolume);
                }
            }


            //for each button in the scene
            foreach (Button button in allGameObjects)
            {
              
                if (button.name == "LowQuality")
                {
                    //set the low quality button to be the button
                    _lowQualityButton = button;

                    //add a listener to the button
                    _lowQualityButton.onClick.AddListener(() => SetQualityLevel(0));
                }

                if (button.name == "MediumQuality")
                {
                    //set the medium quality button to be the button
                    _mediumQualityButton = button;

                    //add a listener to the button
                    _mediumQualityButton.onClick.AddListener(() => SetQualityLevel(1));
                }

                if (button.name == "HighQuality")
                {
                    //set the high quality button to be the button
                    _highQualityButton = button;

                    //add a listener to the button
                    _highQualityButton.onClick.AddListener(() => SetQualityLevel(2));
                }


                //if the button is the no motion blur button
                if (button.name == "NoMotionBlur")
                {
                    //set the motion blur off button to be the button
                    _motionBlurOffButton = button;

                    //add a listener to the button
                    _motionBlurOffButton.onClick.AddListener(() => SetMotionBlur(0));
                }

                //if the button is the low
                if (button.name == "LowMotionBlur")
                {
                    //set the motion blur low button to be the button
                    _motionBlurLowButton = button;

                    //add a listener to the button
                    _motionBlurLowButton.onClick.AddListener(() => SetMotionBlur(1));
                }

                if (button.name == "MediumMotionBlur")
                {
                    //set the motion blur medium button to be the button
                    _motionBlurMediumButton = button;

                    //add a listener to the button
                    _motionBlurMediumButton.onClick.AddListener(() => SetMotionBlur(2));
                }

                if (button.name == "HighMotionBlur")
                {
                    //set the motion blur high button to be the button
                    _motionBlurHighButton = button;

                    //add a listener to the button
                    _motionBlurHighButton.onClick.AddListener(() => SetMotionBlur(3));
                }
            }

        }
    }


    public void OnSceneUnloaded(Scene scene)
    {
      
        //the the currnt scene was the main menu and it changes...
        if (scene == SceneManager.GetSceneByBuildIndex(0))
        {
            //remove listeners from the buttons
            _lowQualityButton?.onClick.RemoveAllListeners();
            _mediumQualityButton?.onClick.RemoveAllListeners();
            _highQualityButton?.onClick.RemoveAllListeners();


            _motionBlurOffButton?.onClick.RemoveAllListeners();
            _motionBlurLowButton?.onClick.RemoveAllListeners();
            _motionBlurMediumButton?.onClick.RemoveAllListeners();
            _motionBlurHighButton?.onClick.RemoveAllListeners();

            //remove listeners from the sliders
            _sensitivitySlider?.onValueChanged.RemoveAllListeners();
            _brightnessSlider?.onValueChanged.RemoveAllListeners();
            _masterVolumeSlider?.onValueChanged.RemoveAllListeners();
            _musicVolumeSlider?.onValueChanged.RemoveAllListeners();
            _sfxVolumeSlider?.onValueChanged.RemoveAllListeners();

        }

        //if the current scene was the bedroom scene and it changes...
        else if (scene == SceneManager.GetSceneByBuildIndex(1))
        {
            //remove listeners from the buttons
            _lowQualityButton?.onClick.RemoveAllListeners();
            _mediumQualityButton?.onClick.RemoveAllListeners();
            _highQualityButton?.onClick.RemoveAllListeners();


            _motionBlurOffButton?.onClick.RemoveAllListeners();
            _motionBlurLowButton?.onClick.RemoveAllListeners();
            _motionBlurMediumButton?.onClick.RemoveAllListeners();
            _motionBlurHighButton?.onClick.RemoveAllListeners();

            //remove listeners from the sliders
            _sensitivitySlider?.onValueChanged.RemoveAllListeners();
            _brightnessSlider?.onValueChanged.RemoveAllListeners();
            _masterVolumeSlider?.onValueChanged.RemoveAllListeners();
            _musicVolumeSlider?.onValueChanged.RemoveAllListeners();
            _sfxVolumeSlider?.onValueChanged.RemoveAllListeners();
        }

    }


   
   

    public int GetQualityLevel()
    {
        //returns the current quality level
        return QualitySettings.GetQualityLevel();
    }

    public float GetBrightnessLevel()
    {
        //returns the current brightness level
        Volume volume = this.GetComponent<Volume>();
        if (volume.profile.TryGet<ColorAdjustments>(out ColorAdjustments brightness))
        {
            return brightness.postExposure.value;
        }
        return 0; // Default value if not found
    }

    public int GetMotionBlurLevel()
    {
        //returns the current motion blur level
        Volume volume = this.GetComponent<Volume>();
        if (volume.profile.TryGet<MotionBlur>(out MotionBlur motionBlur))
        {
            return (int)motionBlur.quality.value;
        }
        return 0; // Default value if not found
    }


    


    public void AdjustSensitivity(float value)
    {
       PlayerInputBehavior.sensitivity = Mathf.FloorToInt(value);
    }

    public void AdjustBrightness(float value)
    {
        Volume volume = this.GetComponent<Volume>();
        if (volume.profile.TryGet<ColorAdjustments>(out ColorAdjustments brightness))
        {
            brightness.postExposure.value = value; // Adjust the brightness level
        }
    }

   

   

    //get the sentitivity level and update the slider
    public float GetSensitivityLevel()
    {
        return PlayerInputBehavior.sensitivity;
    }



    public void SetQualityLevel(int level)
    {
        //sets the quality level to the given level 
        QualitySettings.SetQualityLevel(level);

    }

    public void SetMotionBlur(int level)
    {

        if (level == 0)
        {
            //set the motion blur in the volume profile to off
            Volume volume = this.GetComponent<Volume>();
            if (volume.profile.TryGet<MotionBlur>(out MotionBlur motionBlur))
            {
                motionBlur.quality.value = MotionBlurQuality.Low; // Set low quality
                motionBlur.intensity.value = 0f; // Set to no intensity
            }


        }
        else if (level == 1)
        {
           //set the motion blur in the volume profile to low
            Volume volume = this.GetComponent<Volume>();
            if (volume.profile.TryGet<MotionBlur>(out MotionBlur motionBlur))
            {
                motionBlur.quality.value = MotionBlurQuality.Low; // Set low quality
                motionBlur.intensity.value = 0.2f; // Set low intensity
            }
         
        }
        else if (level == 2)
        {
            //set the motion blur in the volume profile to medium
            Volume volume = this.GetComponent<Volume>();
            if (volume.profile.TryGet<MotionBlur>(out MotionBlur motionBlur))
            {
                motionBlur.quality.value = MotionBlurQuality.Medium; // Set medium quality
                motionBlur.intensity.value = 0.5f; // Set medium intensity
            }
        }
        else if(level == 3)
        {
            //set the motion blur in the volume profile to high
            Volume volume = this.GetComponent<Volume>();
            if (volume.profile.TryGet<MotionBlur>(out MotionBlur motionBlur))
            {
                motionBlur.quality.value = MotionBlurQuality.High; // Set high quality
                motionBlur.intensity.value = 1f; // Set high intensity
            }
        }

        
    }


}
