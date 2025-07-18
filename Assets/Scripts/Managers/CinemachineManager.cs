using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;


public class CinemachineManager : MonoBehaviour
{
    public static CinemachineManager instance;

    //delegate for cinemachine manager
    public delegate void CinemachineEventHandler();

    //Events for the cinemachine event handler
    public static event CinemachineEventHandler onPlayerSleepingVCamActivated;
    public static event CinemachineEventHandler onPlayerSleepingVCamDeactivated;
    

    [Header("Cinemachine Brain")]
    [SerializeField] private CinemachineBrain _cmBrain;



    [Header("Cinemachine Cams in Current Scene")]
    [SerializeField] private CinemachineCamera[] _cmCams;




    private void OnEnable()
    {

        SceneManager.sceneLoaded += OnSceneLoaded;
        CinemachineCore.CameraActivatedEvent.AddListener(OnCamActivated);
        CinemachineCore.CameraDeactivatedEvent.AddListener(OnCamDeactivated);

        SleepBehavior.onPlayerOpenEyes += TransitionToPlayerVCam;
        SleepBehavior.onPlayerCloseEyes += TransitionToPlayerSleepingVCam;

    }



    private void OnDisable()
    {

        SceneManager.sceneLoaded -= OnSceneLoaded;
        CinemachineCore.CameraActivatedEvent.RemoveListener(OnCamActivated);
        CinemachineCore.CameraDeactivatedEvent.RemoveListener(OnCamDeactivated);


        SleepBehavior.onPlayerOpenEyes -= TransitionToPlayerVCam;
        SleepBehavior.onPlayerCloseEyes -= TransitionToPlayerSleepingVCam;
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



    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        //if the scene is the main menu scene
        if (scene == SceneManager.GetSceneByBuildIndex(0))
        {
            //get a list of all the vcams in the main menu scene
            _cmCams = GameObject.FindObjectsByType<CinemachineCamera>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

            //find the cinemachine brain
            _cmBrain = GameObject.FindFirstObjectByType<CinemachineBrain>();

           

        }

        //if the scene is the bedroom scene
        else if (scene == SceneManager.GetSceneByBuildIndex(1))
        {
            //get a list of all the vcams in the bedroom scene
            _cmCams = GameObject.FindObjectsByType<CinemachineCamera>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

            //find the cinemachine brain
            _cmBrain = GameObject.FindFirstObjectByType<CinemachineBrain>();

        }
    }


    public void OnCamActivated(ICinemachineCamera.ActivationEventParams evt)
    {
        //if the active camera is the playera sleeping vCam
        if (_cmBrain.ActiveVirtualCamera.Name == "Player Sleeping VCam")
        {
            //play the onPlayerSleeping vcam activated event
            onPlayerSleepingVCamActivated?.Invoke();
        }
    }

    public void OnCamDeactivated(ICinemachineMixer cmMixer, ICinemachineCamera cmCam)
    {
        //if the player sleeping vcam is no longer the active camera
        if (_cmBrain.ActiveVirtualCamera.Name != "Player Sleeping VCam")
        {
            //play the on player sleeping vcam deactivated event
            onPlayerSleepingVCamDeactivated?.Invoke();
        }
    }



    //hard sets the current vcam
    public void SetVCam(string name)
    {
        //first iterate through the list of cameras
        for (int i = 0; i < _cmCams.Length; i++)
        {
            if (_cmCams[i].name == name)
            {
                _cmCams[i].GetComponent<CinemachineCamera>().enabled = true;
            }
        }
    }


   


    public void TransitionToAnotherCamera(string name)
    {
        // Get the currently active virtual camera and deactivate it
        if (_cmBrain.ActiveVirtualCamera is CinemachineCamera activeCam)
        {
            activeCam.enabled = false;
        }

        // Iterate through the list of cameras
        for (int i = 0; i < _cmCams.Length; i++)
        {
            // If the given name is in the list of cams...
            if (_cmCams[i].Name == name)
            {
                Debug.Log($"Transitioning to camera: {name}");

                

                //debug log the blend speed
                Debug.Log($"Blend Speed: {_cmBrain.DefaultBlend.Time}");

                _cmCams[i].GetComponent<CinemachineCamera>().enabled = true;
            }
        }
    }



    public void TransitionToPlayerVCam()
    {
        TransitionToAnotherCamera("Player VCam");
    }

    public void TransitionToPlayerSleepingVCam()
    {
        TransitionToAnotherCamera("Player Sleeping VCam");
    }

     //resets the vcams (use when exiting scenes)
    public void ResetVCams()
    {
        for (int i = 0; i < _cmCams.Length; i++)
        {
            _cmCams[i].enabled = true;
        }
    }
}
