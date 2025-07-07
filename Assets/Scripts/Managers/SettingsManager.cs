using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance;

    //delegates
    public delegate void SettingsEventHandler();



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





    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //if the scene is the main menu
        if (scene == SceneManager.GetSceneByBuildIndex(0))
        {
            Debug.Log("Find settings for the main menu");
            
        }

        //else if the scene is the bedroom scene
        else if (scene == SceneManager.GetSceneByBuildIndex(1))
        {
            Debug.Log("Find settings for the bedroom scene");
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


   

   
}
