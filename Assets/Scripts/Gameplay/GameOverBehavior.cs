using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverBehavior : MonoBehaviour
{
    public static GameOverBehavior instance;

    public GameObject gameOverScreen;
    public bool gameOver = false;     //A boolean used to determind if the game is over or not

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


    private void Start()
    {
    
        gameOverScreen.SetActive(false);
    }

    private void Update()
    {
        if (gameOver == true)
        {
            //show game over stuff
            gameOverScreen.SetActive(true);
        }
        else if (gameOver == false)
        {
            //hide game over stuff
            gameOverScreen.SetActive(false);

            
        }
    }


    


    //controls the game over screen
    public static bool SetGameOver(bool gameOver)
    {
        GameOverBehavior.instance.gameOver = gameOver;
        Cursor.visible = true;
        return gameOver;

    }


}
