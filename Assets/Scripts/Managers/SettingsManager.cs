using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MotionBlur = UnityEngine.Rendering.Universal.MotionBlur;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance;

    //delegates
    public delegate void SettingsEventHandler();


  


    [Header("Post Processing Buttons")]
    public Button motionBlurOffButton;
    public Button motionBlurLowButton;
    public Button motionBlurMediumButton;
    public Button motionBlurHighButton;


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

            //for each button in the scene
            foreach (Button button in allGameObjects)
            {
                //if the button is the no motion blur button
                if (button.name == "NoMotionBlurButton")
                {
                    //set the motion blur off button to be the button
                    motionBlurOffButton = button;

                    //add a listener to the button
                    motionBlurOffButton.onClick.AddListener(() => SetMotionBlur(0));
                }

                //if the button is the low
                if (button.name == "LowMotionBlurButton")
                {
                  //set the motion blur low button to be the button
                    motionBlurLowButton = button;

                    //add a listener to the button
                    motionBlurLowButton.onClick.AddListener(() => SetMotionBlur(1));
                }

                if (button.name == "MediumMotionBlurButton")
                {
                    //set the motion blur medium button to be the button
                    motionBlurMediumButton = button;

                    //add a listener to the button
                    motionBlurMediumButton.onClick.AddListener(() => SetMotionBlur(2));
                }

                if (button.name == "HighMotionBlurButton")
                {
                    //set the motion blur high button to be the button
                    motionBlurHighButton = button;

                    //add a listener to the button
                    motionBlurHighButton.onClick.AddListener(() => SetMotionBlur(3));
                }
            }

        }

        //else if the scene is the bedroom scene
        else if (scene == SceneManager.GetSceneByBuildIndex(1))
        {
            
        }

    }


    public void OnSceneUnloaded(Scene scene)
    {
      
        //the the currnt scene was the main menu and it changes...
        if (scene == SceneManager.GetSceneByBuildIndex(0))
        {
           //remove listeners from the buttons
            motionBlurOffButton.onClick.RemoveAllListeners();
            motionBlurLowButton.onClick.RemoveAllListeners();
            motionBlurMediumButton.onClick.RemoveAllListeners();
            motionBlurHighButton.onClick.RemoveAllListeners();
           

        }

        //if the current scene was the bedroom scene and it changes...
        else if (scene == SceneManager.GetSceneByBuildIndex(1))
        {
               

        }

    }




    private void Start()
    {
        //get the quality level on startup
        GetQualityLevel();
    }



    public void AdjustSensitivity(float value)
    {
       PlayerInputBehavior.sensitivity = value;
    }

    public void AdjustBrightness(float value)
    {
      
    }

   

    public void GetQualityLevel()
    {
        int qualityLevel = QualitySettings.GetQualityLevel();
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
