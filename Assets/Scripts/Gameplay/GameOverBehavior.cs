using UnityEngine;
using UnityEngine.UI;


public class GameOverBehavior : MonoBehaviour
{
    //delegates
    public delegate void GameOver();

    //Events
    public static GameOver onGameOver;    


    //A boolean used to determind if the game is over or not
    public static bool gameOver = false;


    [Header("Buttons")]
    [SerializeField] private Button _retryButton;
    [SerializeField] private Button _quitButton;

   
    [Header("Game Objects")]
    [SerializeField] private GameObject gameOverScreen;    //The game over screen



    private void OnEnable()
    {

        //adds the button events
        _retryButton.onClick.AddListener(() => LevelManager.instance.ReloadScene());
        _quitButton.onClick.AddListener(() => Application.Quit());


        //Resets all the variables when the game is started
        GameManager.onGameStarted += ResetVariables;
        onGameOver += SetGameOver;
       
        
    }

    private void OnDisable()
    {
        //removes the button events
        _retryButton.onClick.RemoveListener(() => LevelManager.instance.ReloadScene());
        _quitButton.onClick.RemoveListener(() => Application.Quit());


        //Removes all the variables when the game is ended
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
