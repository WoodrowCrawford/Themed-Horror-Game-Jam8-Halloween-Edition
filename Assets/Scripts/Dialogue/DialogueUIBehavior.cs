using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueUIBehavior : MonoBehaviour
{
    //Gets a static version of this class (so that other classes can use it)
    public static DialogueUIBehavior instance;

    private ResponseHandlerBehavior _responseHandler;
    private TypewritterEffectBehavior _typewritterEffect;

    [Header("Dialogue Box Settings")]
    [SerializeField] private GameObject _dialogueBox;
    [SerializeField] private Image _dialogueBackground;

    
   

    [Header("Dialogue Text Settings")]
    [SerializeField] private TMP_Text _textLabel;
    [SerializeField] private TMP_ColorGradient _daytimeTextColorGradient;
    [SerializeField] private TMP_ColorGradient _nighttimeTextColorGradient;


    [Header("Response Box Settings")]
    [SerializeField] private Image _responseBoxBG;



    [Header("Response Text Settings")]
    [SerializeField] private TMP_Text _responseText;



    //A bool to check if the dialogue box is open
    public static bool IsOpen { get; private set; }



    private void Awake()
    {
       
        //Gets the components on awake
        _typewritterEffect = GetComponent<TypewritterEffectBehavior>();
        _responseHandler = GetComponent<ResponseHandlerBehavior>(); 

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
       
        //Closes the dialogue box on Startup
        CloseDialogueBox();
    }


    private void Update()
    {
        UpdateDialogueBoxLook();
    }


    //A function that shows the dialogue box
    public void ShowDialogue(DialogueObjectBehavior dialogueObject)
    {
        if(GameManager.instance.currentGameMode == GameManager.GameModes.MAIN_MENU)
        {
            return;
        }

        else if(GameManager.instance.currentGameMode != GameManager.GameModes.MAIN_MENU)
        {
            //Stops time and shows the Cursor while the dialogue is open
            Cursor.visible = true;
            Time.timeScale = 0.0f;

            //disables pausing while the dialogue is open
            PlayerInputBehavior.playerCanPause = false;


            //Set to be false so that the player cant interact while dialogue is open
            PlayerInputBehavior.playerCanInteract = false;

            //Sets is open to be true
            IsOpen = true;

            //Enables the dialogue box game object
            _dialogueBox.SetActive(true);

            //Starts the step through dialogue coroutine
            StartCoroutine(StepThroughDialogue(dialogueObject));
        }
        
    }



    

    //A function that adds response events
    public void AddResponseEvents(ResponseEvent[] responseEvents)
    {
        _responseHandler.AddResponseEvents(responseEvents);
    }

    private IEnumerator StepThroughDialogue(DialogueObjectBehavior dialogueObject)
    {
        //gets the length of the dialogue in dialogueObject
        for(int i =  0; i < dialogueObject.Dialogue.Length; i++)
        {
           

            string dialogue = dialogueObject.Dialogue[i];
            yield return _typewritterEffect.Run(dialogue, _textLabel);

            if (i == dialogueObject.Dialogue.Length - 1 && dialogueObject.HasResponses)
            {
                break;
            }

         

            //Waits until the given input has been pressed before continuing 
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0) && !PauseSystem.isPaused);
        }

        //if the dialogue has responses
        if (dialogueObject.HasResponses)
        {
            //Show the responses 
            _responseHandler.ShowResponses(dialogueObject.Responses);
        }
        else
        {
            //If it doesnt have any responses then close the dialogue box
            CloseDialogueBox();
        }

        
    }

    
    //A function that closes the dialogue box
    public void CloseDialogueBox()
    {
        //Resumes time and hides the cursor
        Cursor.visible = false;
        Time.timeScale = 1.0f;

        //Sets is open to false
        IsOpen = false;

        //enables the player to pause the game when the dialogue box is closed
        PlayerInputBehavior.playerCanPause = true;

        //Set to be true so that the player can interact while dialogue is open
        PlayerInputBehavior.playerCanInteract = true;
        

        //disables the dialogue box game object (hides it)
        _dialogueBox.SetActive(false);

        //Sets the text label's text to be empty
        _textLabel.text = string.Empty;
    } 


    public void UpdateDialogueBoxLook()
    {
        //if it is day time...
        if(GraphicsBehavior.instance.IsDayTime)
        {

            //set the text to be the normal version
            _textLabel.colorGradientPreset = _daytimeTextColorGradient;


            //set the response text to be the normal version
            _responseText.colorGradientPreset = _daytimeTextColorGradient;
        }
        
        //if it is nighttime...
        else if (GraphicsBehavior.instance.IsNightTime)
        {

            //set the text to be the dark version
            _textLabel.colorGradientPreset = _nighttimeTextColorGradient;


            

            //set the response text to be the dark version
            _responseText.colorGradientPreset = _nighttimeTextColorGradient;
        }
    }
}
