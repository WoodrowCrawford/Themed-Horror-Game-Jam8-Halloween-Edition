using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
public class TimelineManager : MonoBehaviour
{
    public static TimelineManager instance;


    [Header("Current Director")]
    public PlayableDirector currentPlayableDirector;
    [SerializeField] private bool _cutsceneIsPlaying = false;

    [Header("Stored Directors")]
    public PlayableDirector  dummyPlayableDirector;
    public PlayableDirector ghoulPlayableDirector;
    public PlayableDirector clownPlayableDirector;
   

    //delegate
    public delegate void Cutscene();
    
  
    //Events
    public static Cutscene onPlayDummyJumpscare;
    public static Cutscene onPlayGhoulJumpscare;
    public static Cutscene onPlayClownJumpscare;

    public static event Cutscene onCutscenePlayed;
    public static event Cutscene onCutsceneStopped;


    public bool CutsceneIsPlaying { get { return _cutsceneIsPlaying; } }
   

    

    private void OnEnable()
    {
        
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;


        onCutscenePlayed += () => _cutsceneIsPlaying = true;
        onCutsceneStopped += () => _cutsceneIsPlaying = false;
       

        onPlayDummyJumpscare += () => PlayCutscene(dummyPlayableDirector);
        onPlayGhoulJumpscare += () => PlayCutscene(ghoulPlayableDirector);
        onPlayClownJumpscare += () => PlayCutscene(clownPlayableDirector);
    }

    

           

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;


        onCutscenePlayed -= () => _cutsceneIsPlaying = true;
        onCutsceneStopped -= () => _cutsceneIsPlaying = false;


        onPlayDummyJumpscare -= () => PlayCutscene(dummyPlayableDirector);
        onPlayGhoulJumpscare -= () => PlayCutscene(ghoulPlayableDirector);
        onPlayClownJumpscare -= () => PlayCutscene(clownPlayableDirector);
    }


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
            dummyPlayableDirector = null;
            ghoulPlayableDirector = null;
            clownPlayableDirector = null;
        }

        //if the scene is the bedroom scene
        else if (scene == SceneManager.GetSceneByBuildIndex(1))
        {
            //find the directables
            dummyPlayableDirector = GameObject.FindGameObjectWithTag("DummyJumpscare").GetComponent<PlayableDirector>();
            ghoulPlayableDirector = GameObject.FindGameObjectWithTag("GhoulJumpscare").GetComponent<PlayableDirector>();
            clownPlayableDirector = GameObject.FindGameObjectWithTag("ClownJumpscare").GetComponent<PlayableDirector>();
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
            
        }
    }


    public void PlayCutscene(PlayableDirector playableDirector)
    {
        //if a cutscene is not currently playing...
        if (!_cutsceneIsPlaying)
        {
            //set the current director to be the director that is playing
            currentPlayableDirector = playableDirector;

            currentPlayableDirector.Play(playableDirector.playableAsset);
            
        }
        else
        {
            return;
        }

     
    }
}
