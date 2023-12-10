using UnityEngine;


public class GameOverBehavior : MonoBehaviour
{
    

    //delegates
    public delegate void GameOver();

    //Events
    public static event GameOver onGameOver;     //uses to call game over stuff


    //A boolean used to determind if the game is over or not
    public static bool gameOver = false;

    //The game over screen
    [SerializeField] private GameObject gameOverScreen;



    


    private void OnEnable()
    {
        GameManager.onGameStarted += ResetVariables;
    }

    private void OnDisable()
    {
        GameManager.onGameStarted -= ResetVariables;
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


        //show the game over screen
        gameOverScreen.gameObject.SetActive(true);


        //calls the event onGameOver
        onGameOver?.Invoke();

        Cursor.visible = true;


    }

    public void ResetVariables()
    {
        gameOver = false;
        gameOverScreen.SetActive(false);
    }

   
}
