using UnityEngine;
using UnityEngine.SceneManagement;


public class GraphicsBehavior : MonoBehaviour
{
    //Graphics behavior used to control graphics and visuals in the game/////


    public static GraphicsBehavior instance;

    //delegates
    public delegate void TimeChange();

    //events
    public static TimeChange OnDayTime;
    public static TimeChange OnNightTime;

   
    public GameObject Graphics; //the game object used for graphics
    public GameObject CurrentPostProcessingObject; //the pp used for the lighting
    public GameObject Sun;  //The sun used for lighting




    public bool IsDayTime = false;
    public bool IsNightTime = false;




    private void OnEnable()
    {
        GameManager.onGameStarted += FindSun;
        GameManager.onGameEnded += EndTest;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        GameManager.onGameStarted -= FindSun;
        GameManager.onGameEnded -= FindSun;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Awake()
    {
        //Finds the graphics game object
        Graphics = GameObject.FindGameObjectWithTag("Graphics");

        //Finds the Sun game object
        //Sun = GameObject.FindGameObjectWithTag("Sun");

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
            CurrentPostProcessingObject = GameObject.Find("PostProcessing");
        }
    }


    //Sets the scene to be daytime
    public void SetDayTime()
    {
        //Enables the light component in the sun object
        //Sun.GetComponent<Light>().enabled = true;

  
       

        IsDayTime = true;
        IsNightTime = false;

        //sets the graphic game object to be false
       // CurrentPostProcessingObject.SetActive(false);

        Debug.Log("Set to day time!!!!!");
    }

    //Sets the scene to be nighttime
    public void SetNightTime()
    {
        //Disables the light component in the sun object
       //Sun.GetComponent<Light>().enabled = false;

       

        IsNightTime = true;
        IsDayTime = false;

        //sets the graphic game object to be true
       // CurrentPostProcessingObject.SetActive(true);
    }


    public void EndTest()
    {
        Sun = null;
        Debug.Log("Graphics behavior is playing what happens when the game ends");
    }

    public void FindSun()
    {
        Sun = GameObject.FindGameObjectWithTag("Sun");
        Debug.Log("hey i should find the sun now");
    }
}