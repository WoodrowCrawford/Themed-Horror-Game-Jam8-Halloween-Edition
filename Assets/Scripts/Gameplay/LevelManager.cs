using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;


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
        var scene = SceneManager.LoadSceneAsync(sceneName);

     
    }

   
    //Reloads the current scene
    public void ReloadScene()
    {
        var scene = SceneManager.GetActiveScene();
        PauseSystem.instance.TogglePauseMenu();
        SceneManager.LoadScene(scene.name);

        DayManager.instance.ResetInitializers();
    }

    
}
