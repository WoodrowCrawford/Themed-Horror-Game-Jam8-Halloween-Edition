using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;



public class GraphicsBehavior : MonoBehaviour
{
    //Graphics behavior used to control graphics and visuals in the game/////


    public static GraphicsBehavior instance;

    //delegates
    public delegate void TimeChange();

   

    

   
    public GameObject Graphics; //the game object used for graphics
    public GameObject CurrentPostProcessingObject; //the pp used for the lighting
    public GameObject Sun;  //The sun used for lighting
    public GameObject Moon; //The moon used for lighting 

    [Header("Profiles")]
    public VolumeProfile DayTimeVolume;
    public VolumeProfile NightTimeVolume;

  


    public bool IsDayTime = false;
    public bool IsNightTime = false;




    private void OnEnable()
    {
        GameManager.onGameStarted += FindSunAndMoon;
        GameManager.onGameEnded += EndTest;

        DayManager.OnDayTime += SetDayTime;
        DayManager.OnNightTime += SetNightTime;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        GameManager.onGameStarted -= FindSunAndMoon;
        GameManager.onGameEnded -= EndTest;

        DayManager.OnDayTime -= SetDayTime;
        DayManager.OnNightTime -= SetNightTime;

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Awake()
    {
        //Finds the graphics game object
        Graphics = GetComponent<GraphicsBehavior>().gameObject;

        

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
        Debug.Log("On scene loaded:" + scene.name);
        Debug.Log("mode");

        //if the scene is the main menu scene
        if (scene == SceneManager.GetSceneByBuildIndex(0))
        { 
            CurrentPostProcessingObject = GameObject.Find("Post Processing");
        }

        //if the scene is the bedroom scene
        else if (scene == SceneManager.GetSceneByBuildIndex(1))
        {
            CurrentPostProcessingObject = GameObject.Find("Post Processing");

            //find the sun and moon 
             FindSunAndMoon();
        }
    }


    //Sets the scene to be daytime
    public void SetDayTime()
    {

        //set is day time to true
        IsDayTime = true;
        IsNightTime = false;

        //get the light component in the sun and turn it on
        Sun.GetComponent<Light>().enabled = true;

        //set the pp profile
        CurrentPostProcessingObject.GetComponent<Volume>().profile = DayTimeVolume;


        //Disable the light component in the moon object
         Moon.GetComponent<Light>().enabled = true;

  
       

        

        //sets the graphic game object to be false
       // CurrentPostProcessingObject.SetActive(false);

        Debug.Log("Set to day time!!!!!");
    }

    //Sets the scene to be nighttime
    public void SetNightTime()
    {
       

        //set is day time to true
        IsDayTime = false;
        IsNightTime = true;

        //get the light component in the moon and turn it on
        Moon.GetComponent<Light>().enabled = true;

        CurrentPostProcessingObject.GetComponent<Volume>().profile = NightTimeVolume;

        //Disable the light component in the sun object
        Sun.GetComponent<Light>().enabled = false;




    

        //sets the graphic game object to be false
        // CurrentPostProcessingObject.SetActive(false);

        Debug.Log("Set to night time!!!!!");
    }


    public void EndTest()
    {
        Sun = null;
        Moon = null;
        Debug.Log("Graphics behavior is playing what happens when the game ends");
    }

    public void FindSunAndMoon()
    {
        Sun = GameObject.Find("Sun");
        Moon = GameObject.Find("Moon");
    }

    
}