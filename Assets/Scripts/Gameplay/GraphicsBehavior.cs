using UnityEngine;

public class GraphicsBehavior : MonoBehaviour
{
    //Graphics behavior used to control graphics and visuals in the game/////


    public static GraphicsBehavior instance;

    //delegates
    public delegate void  TimeChange();


    //events
    public static TimeChange OnDayTime;
    public static TimeChange OnNightTime;

   
    public GameObject Graphics; //the game object used for graphics
    public GameObject Sun;  //The sun used for lighting


    public bool IsDayTime = false;
    public bool IsNightTime = false;




    private void OnEnable()
    {
        GameManager.onGameStarted += Test;
    }

    private void OnDisable()
    {
        GameManager.onGameStarted -= Test;
    }

    private void Awake()
    {
        //Finds the graphics game object
        Graphics = GameObject.FindGameObjectWithTag("Graphics");

        //Finds the Sun game object
        Sun = GameObject.FindGameObjectWithTag("Sun");

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

   


    //Sets the scene to be daytime
    public void SetDayTime()
    {
        //Enables the light component in the sun object
        Sun.GetComponent<Light>().enabled = true;
        
        IsDayTime = true;
        IsNightTime = false;

        //sets the graphic game object to be false
        Graphics.SetActive(false);

        Debug.Log("Set to day time!!!!!");
    }

    //Sets the scene to be nighttime
    public void SetNightTime()
    {
        //Disables the light component in the sun object
        Sun.GetComponent<Light>().enabled = false;

        IsNightTime = true;
        IsDayTime = false;

        //sets the graphic game object to be true
        Graphics.SetActive(true);
    }


    public void Test()
    {
        Debug.Log("hey i should find the sun now");
    }
}
