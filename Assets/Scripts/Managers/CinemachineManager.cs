using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using static GameManager;
using Unity.VisualScripting;

public class CinemachineManager : MonoBehaviour
{
    public static CinemachineManager instance;
   


    public Scene currentScene;

    [Header("Cinemachine Brain")]
    [SerializeField] private CinemachineBrain _cmBrain;

    [Header("VCams in Current Scene")]
    [SerializeField] private CinemachineVirtualCamera[] _vCams;




    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        

    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
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
            _vCams = GameObject.FindObjectsByType<CinemachineVirtualCamera>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

            //find the cinemachine brain
            _cmBrain = GameObject.FindFirstObjectByType<CinemachineBrain>();
        }

        //if the scene is the bedroom scene
        else if (scene == SceneManager.GetSceneByBuildIndex(1))
        {
            //get a list of all the vcams in the bedroom scene
            _vCams = GameObject.FindObjectsByType<CinemachineVirtualCamera>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

            //find the cinemachine brain
            _cmBrain = GameObject.FindFirstObjectByType<CinemachineBrain>();

        }
    }

   
}
