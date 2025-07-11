using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TimelineManager : MonoBehaviour
{
    public static TimelineManager instance;

 


    [Header("Current Director")]
    public PlayableDirector currentPlayableDirector;
    [SerializeField] private bool _cutsceneIsPlaying = false;

    [Header("Stored Directors Main menu")]
    public PlayableDirector mainMenuDirector;


    [Header("Main Menu Cutscenes")]
    public PlayableAsset _mainMenuTransitionToBook;

    [Header("Stored Directors Bedroom")]

    public PlayableDirector dummy1PlayableDirector;
    public PlayableDirector dummy2PlayableDirector;

    public PlayableDirector ghoulPlayableDirector;
    public PlayableDirector clownPlayableDirector;


    [Header("Stored Jumpscares")]
    public PlayableAsset dummy1JumpscareAsset;
    public PlayableAsset dummy2JumpscareAsset;

    public PlayableAsset ghoulJumpscareAsset;
    public PlayableAsset clownJumpscareAsset;


    //delegate
    public delegate void CutsceneEventHandler();


    //Events
   
    public static CutsceneEventHandler onPlayDummy1Jumpscare;
    public static CutsceneEventHandler onPlayDummy2Jumpscare;


    public static CutsceneEventHandler onPlayGhoulJumpscare;
    public static CutsceneEventHandler onPlayClownJumpscare;

    public static event CutsceneEventHandler onCutscenePlayed;
    public static event CutsceneEventHandler onCutsceneStopped;



  



    public bool CutsceneIsPlaying { get { return _cutsceneIsPlaying; } }
    public void SetCutsceneIsPlayingToTrue() { _cutsceneIsPlaying = true; }
    public void SetCutsceneIsPlayingToFalse() { _cutsceneIsPlaying = false; }



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


        onCutscenePlayed += SetCutsceneIsPlayingToTrue;
        onCutsceneStopped += SetCutsceneIsPlayingToFalse;


        onPlayDummy1Jumpscare += PlayDummy1Jumpscare;
        onPlayDummy2Jumpscare += PlayDummy2Jumpscare;

        onPlayGhoulJumpscare += PlayGhoulJumpscare;
        onPlayClownJumpscare += PlayClownJumpscare;
    }

    

           

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;


        onCutscenePlayed -= SetCutsceneIsPlayingToTrue;
        onCutsceneStopped -= SetCutsceneIsPlayingToFalse;

        onPlayDummy1Jumpscare -= PlayDummy1Jumpscare;
        onPlayDummy2Jumpscare -= PlayDummy2Jumpscare;

        onPlayGhoulJumpscare -= PlayGhoulJumpscare;
        onPlayClownJumpscare -= PlayClownJumpscare;
    }



   




    private void Update()
    {

        if(currentPlayableDirector == null)
        {
            return;
        }

        else if(currentPlayableDirector.state == PlayState.Playing)
        {
            //invoke current directable event here that it is playing
            onCutscenePlayed?.Invoke();
            
        }
        else if(currentPlayableDirector.state != PlayState.Playing)
        {
            onCutsceneStopped?.Invoke();
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("On scene loaded:" + scene.name);
        Debug.Log("mode");

        //if the scene is the main menu scene
        if (scene == SceneManager.GetSceneByBuildIndex(0))
        {
            mainMenuDirector = GameObject.Find("MainMenuDirector").gameObject.GetComponent<PlayableDirector>();

            dummy1PlayableDirector = null;
            dummy2PlayableDirector = null;

            ghoulPlayableDirector = null;
            clownPlayableDirector = null;
        }

        //if the scene is the bedroom scene
        else if (scene == SceneManager.GetSceneByBuildIndex(1))
        {
            mainMenuDirector = null;

            dummy1PlayableDirector = GameObject.Find("Dummy1").GetComponent<PlayableDirector>();
            dummy2PlayableDirector = GameObject.Find("Dummy2").GetComponent<PlayableDirector>();

            ghoulPlayableDirector = GameObject.FindFirstObjectByType<GhoulStateManager>().GetComponent<PlayableDirector>();
            clownPlayableDirector = GameObject.FindFirstObjectByType<ClownStateManager>().GetComponent<PlayableDirector>();
        }
    }

    public void OnSceneUnloaded(Scene scene)
    {
        Debug.Log("On Scene Unloaded:" + scene.name);
        Debug.Log("mode");

        //the the currnt scene was the main menu and it changes...
        if (scene == SceneManager.GetSceneByBuildIndex(0))
        {
            //unload all main menu stuff
            Debug.Log("Do unloading stuff for main menu scene");
        }

        //if the current scene was the bedroom scene and it changes...
        else if (scene == SceneManager.GetSceneByBuildIndex(1))
        {
            return;
        }
    }


    //Plays a normal cutscene
    public void PlayCutscene(PlayableDirector playableDirector, PlayableAsset cutsceneToPlay, DirectorWrapMode wrapMode)
    {
        //if a cutscene is not currently playing...
        if (!CutsceneIsPlaying)
        {
            //set the current director to be the director that is playing
            currentPlayableDirector = playableDirector;

            //set the current cutscene to play to be the playable directors playable asset
            currentPlayableDirector.playableAsset = cutsceneToPlay;

            currentPlayableDirector.Play(playableDirector.playableAsset);

            
            
        }
        else
        {
            Debug.Log("Cant play another one");
            return;
        }
    }


    public void PlayDummy1Jumpscare()
    {
        PlayCutscene(dummy1PlayableDirector, dummy1JumpscareAsset, DirectorWrapMode.None);
    }

    public void PlayDummy2Jumpscare()
    {
        PlayCutscene(dummy2PlayableDirector, dummy2JumpscareAsset, DirectorWrapMode.None);
    }

    public void PlayGhoulJumpscare()
    {
        PlayCutscene(ghoulPlayableDirector, ghoulJumpscareAsset, DirectorWrapMode.None);
    }

    public void PlayClownJumpscare()
    {
        PlayCutscene(clownPlayableDirector, clownJumpscareAsset, DirectorWrapMode.None);
    }
}
