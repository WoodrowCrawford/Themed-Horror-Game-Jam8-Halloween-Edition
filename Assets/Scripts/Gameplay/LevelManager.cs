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
        StartCoroutine(DayManager.instance.ResetInitializers());

     
    }

   
    //Reloads the current scene
    public void ReloadScene()
    {
        var scene = SceneManager.GetActiveScene();
        StartCoroutine(DayManager.instance.ResetInitializers());

        PauseSystem.instance.TogglePauseMenu();
        SceneManager.LoadScene(scene.name);

       
    }

    
}
