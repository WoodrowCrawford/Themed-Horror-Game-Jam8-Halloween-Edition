using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class GraphicsBehavior : MonoBehaviour
{
    //Graphics behavior used to control graphics and visuals in the game/////


    public static GraphicsBehavior instance;

   
    public GameObject Graphics; //the game object used for graphics
    public GameObject Sun;  //The sun used for lighting

   
   


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

    private void Update()
    {
        //Finds the sun game object

       
        
    }


    //Sets the scene to be daytime
    public void SetDayTime()
    {
        //Enables the light component in the sun object
        Sun.GetComponent<Light>().enabled = true;
        
        //sets the graphic game object to be false
        Graphics.SetActive(false);

        Debug.Log("Set to day time!!!!!");
    }

    //Sets the scene to be nighttime
    public void SetNightTime()
    {
        //Disables the light component in the sun object
        Sun.GetComponent <Light>().enabled = false;
        
        //sets the graphic game object to be true
        Graphics.SetActive(true);
    }

}
