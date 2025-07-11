using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;


public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

   

    [Header("Loading Screen Values")]
    [SerializeField] private Canvas _loadingScreenCanvas;
    [SerializeField] private Image _loadingBarFill;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }





    public void LoadScene(string sceneName)
    {
       
        //starts the loading scene corutine
        StartCoroutine(LoadSceneAsync(sceneName));
        
    }


    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);


      

        _loadingScreenCanvas.gameObject.SetActive(true);

        //while the loading screen is not finished
        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);

           _loadingBarFill.fillAmount = progressValue;

            yield return null;
        }

       

        _loadingScreenCanvas.gameObject.SetActive(false);

    }
   
    //Reloads the current scene
    public void ReloadScene()
    {
        //reloads the scene
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

   

    public void ReloadSceneGameOver()
    {
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }


    public void QuitGame()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }
    
}
