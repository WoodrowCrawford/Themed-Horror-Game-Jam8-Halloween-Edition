using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineManager : MonoBehaviour
{
    public TimelineAction TimelineAction;
    public PlayableDirector director;
    public PlayableAsset _currentCutscene;




    private void Awake()
    {
        director= GetComponent<PlayableDirector>();

    }

    // Start is called before the first frame update
    void Start()
    {

        director.Play(_currentCutscene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }





    //TIMELINES/////////////////////////////////////////////////////////////////////////////////////////
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


}