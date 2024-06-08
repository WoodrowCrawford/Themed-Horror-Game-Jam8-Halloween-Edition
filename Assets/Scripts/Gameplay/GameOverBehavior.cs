using TMPro;
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

   
    

    [Header("Game over Screens")]
    [SerializeField] private GameObject _gameOverScreen;    //the game over screen
    [SerializeField] private TMP_Text _gameOverTipsText;    //the game over tips text
    [SerializeField] private Sprite _dummyGameOverScreen;
    [SerializeField] private Sprite _clownGameOverScreen;
    [SerializeField] private Sprite _ghoulGameOverScreen;
   


    public TMP_Text GameOverTipsText { get { return _gameOverTipsText; } set { _gameOverTipsText = value; } }



    private void OnEnable()
    {
        //adds the button events
        _retryButton.onClick.AddListener(() => LevelManager.instance.ReloadScene());
        _quitButton.onClick.AddListener(() => Application.Quit());


        //Resets the game over state when the game is started
        GameManager.onGameStarted += ResetGameOverState;
      
    }

    private void OnDisable()
    {
        //removes the button events
        _retryButton.onClick.RemoveListener(() => LevelManager.instance.ReloadScene());
        _quitButton.onClick.RemoveListener(() => Application.Quit());


        //Removes the reset game over state event
        GameManager.onGameStarted -= ResetGameOverState;
        
       
    }


    private void Start()
    {
        //set game over to be false
        gameOver = false;

        //hide the game over screen
        _gameOverScreen.SetActive(false);
    } 


    public void SetGameOver()
    {
        //sets to be true
        gameOver = true;

        //checks to see what killed the player
        CheckToSeeWhatKilledPlayer();

        //call the on game over event
        onGameOver?.Invoke();


        //shows the cursor
        Cursor.visible = true;

        //show the game over screen
        _gameOverScreen.SetActive(true);
    }

    public void ResetGameOverState()
    {
        gameOver = false;
        _gameOverScreen.SetActive(false);
    }



  public void CheckToSeeWhatKilledPlayer()
    {
        //if the dummy killed the player...
        if(DummyStateManager.dummyKilledPlayer)
        {
            //set the tips text
            _gameOverTipsText.text = "The dummies hate light. Use this to your advantage.";

            //change the game over background here
            _gameOverScreen.GetComponent<Image>().sprite = _dummyGameOverScreen;
        }
        //if the clown killed the player...
        else if (ClownStateManager.clownKilledPlayer)
        {
            //set the tips text
            _gameOverTipsText.text = "Keep the music box playing in order to keep the clown away.";

            //change the game over background here
            _gameOverScreen.GetComponent<Image>().sprite = _clownGameOverScreen;
        }
        
        //if the ghoul killed the player
        else if (GhoulStateManager.ghoulKilledPlayer)
        {
            //set the tips text
            _gameOverTipsText.text = "Make sure to hide whenever the ghoul comes in the room!";

            //change the game over background here
            _gameOverScreen.GetComponent<Image>().sprite = _ghoulGameOverScreen;
        }
    }
  
   
}
