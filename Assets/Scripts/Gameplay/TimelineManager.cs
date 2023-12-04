using System;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.Timeline;

public class TimelineManager : MonoBehaviour
{
   
    public PlayableDirector director;
    public PlayableAsset _currentCutscene;
 

   

    


    private void OnEnable()
    {

        director.played += ctx => CutscenePlayed();
        director.stopped += ctx => CutsceneEnded();
    }

    private void CutsceneEnded()
    {
        Debug.Log("ITs over");
    }

    private void CutscenePlayed()
    {
        Debug.Log("Okay it is playing");
    }

    private void Awake()
    {
        director= GetComponent<PlayableDirector>();

    }

    // Start is called before the first frame update
    void Start()
    {
       
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.jKey.wasPressedThisFrame)
        {
            Debug.Log("Okay play something");
            
        }
    }


    


    //Emitters/////////////////////////////////////////////////////////////////////////////////////////
    public void SwitchCamera()
    {
        Debug.Log("Hey something needs to happen here!!!");
    }

    public void SecondSignalTest()
    {
        Debug.Log("So.....how are you I guess?");
    }

    public void ThirdSignalTest()
    {
        Debug.Log("Ok thats enough what gives????");
    }


    public void Test()
    {
        Debug.Log("Okay it is starting");
    }
    
    public void EndTest()
    {
        Debug.Log("Okay it is over");
    }

    public void Play(TimelineAsset timeline)
    {
        Debug.Log("Okay i should find out how to play" +  timeline);
    }
}
