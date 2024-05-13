using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;



    [Header("Loading Screen Values")]
    [SerializeField] private Image _loadingImage;
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


    


    public async void LoadScene(string sceneName)
    {
        //starts the loading scene corutine
        StartCoroutine(LoadSceneAsync(sceneName));
    }


    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);


      

        _loadingImage.gameObject.SetActive(true);
        _loadingBarFill.gameObject.SetActive(true);

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            


           _loadingBarFill.fillAmount = progressValue;

            yield return null;
        }

        _loadingImage.gameObject.gameObject.SetActive(false);
        _loadingBarFill.gameObject.SetActive(false);

    }
   
    //Reloads the current scene
    public void ReloadScene()
    {
        var scene = SceneManager.GetActiveScene();
     
        PauseSystem.instance.TogglePauseMenu();
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
