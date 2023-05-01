using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class GraphicsBehavior : MonoBehaviour
{
    //Graphics behavior used to control graphics and visuals in the game/////

    public static GraphicsBehavior instance;

    public GameObject Sun;
    public GameObject Graphics; //the game object used for graphics

   
   


    private void Awake()
    {
       

        //Finds the graphics game object
        Graphics = GameObject.FindGameObjectWithTag("Graphics");

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


        Sun= GameObject.FindGameObjectWithTag("Sun");
    }


    //Sets the scene to be daytime
    public void SetDayTime()
    {
        if(Sun != null)
        {
            Sun.SetActive(true);
        }
       
        Graphics.SetActive(false);
    }

    //Sets the scene to be nighttime
    public void SetNightTime()
    {
        if(Sun != null)
        {
            Sun.SetActive(false);
        }
        
        Graphics.SetActive(true);
    }

}
