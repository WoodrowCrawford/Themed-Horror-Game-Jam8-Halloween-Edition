using UnityEngine;


public class GameOverBehavior : MonoBehaviour
{
    //delegates
    public delegate void GameOver();

    //Events
    public static GameOver onGameOver;    


    //A boolean used to determind if the game is over or not
    public static bool gameOver = false;

    //The game over screen
    [SerializeField] private GameObject gameOverScreen;



    private void OnEnable()
    {
        //Resets all the variables when the game is started
        GameManager.onGameStarted += ResetVariables;
        onGameOver += SetGameOver;
       
        
    }

    private void OnDisable()
    {
        GameManager.onGameStarted -= ResetVariables;
        onGameOver -= SetGameOver;
       
    }


    private void Start()
    {
        //set game over to be false
        gameOver = false;

        //hide the game over screen
        gameOverScreen.SetActive(false);
    }

   


    public void SetGameOverScreen(bool active)
    {
        gameOverScreen.SetActive(active);
    }

   


    public void SetGameOver()
    {
        //sets to be true
        gameOver = true;

        //shows the cursor
        Cursor.visible = true;
    }

    public void ResetVariables()
    {
        gameOver = false;
        gameOverScreen.SetActive(false);
    }

   
}
