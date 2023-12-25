using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public class WardrobeBehavior : MonoBehaviour, IInteractable
{
  
    public PlayerInputBehavior playerInputBehavior;
    public Animator animator;

    [Header("Dialogue")]
    [SerializeField] private string _interactionPrompt;
    [SerializeField] private DialogueObjectBehavior _wardrobeDialogue;

    [SerializeField] private GameObject _wardrobeDoorTrigger;
    [SerializeField] private GameObject _player;

    [Header("Wardrobe Positions")]
    [SerializeField] private Transform _insideWardrobePos;
    [SerializeField] private Transform _outsideWardrobePos;
    [SerializeField] private Transform _originalPos;



    public bool actionOnCoolDown = false;
    public bool playerCanOpenWardrobe;
    public bool wardrobeDoorIsOpen = false;
    public bool playerCanGetInWardrobe = false;
    public bool playerIsInWardrobe = false;


    public string InteractionPrompt => _interactionPrompt;

    public DialogueObjectBehavior DialogueObject => _wardrobeDialogue;

    public Transform OriginalPos => _originalPos;

    private void OnEnable()
    {
        GameManager.onGameStarted += test;
    }

    private void OnDisable()
    {
        GameManager.onGameStarted -= test;
    }



    private void Update()
    {
        //Gets the player input behavior when the player is in the bedroom scene
        if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("BedroomScene"))
        {
            playerInputBehavior = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputBehavior>();
        }


        if (playerCanGetInWardrobe && playerInputBehavior.playerControls.OutOfBed.GetInBed.WasPerformedThisFrame())
        {
            ToggleGetInOutOfWardrobe();
        }
    }

    public IEnumerator OpenWardrobeDoor()
    {
        animator.Play("WardrobeDoorOpenAnim");
        actionOnCoolDown = true;
        wardrobeDoorIsOpen = true;
        yield return new WaitForSeconds(1.2f);

        actionOnCoolDown = false;
        
    }

    public IEnumerator CloseWardrobeDoor()
    {
        animator.Play("WardrobeDoorCloseAnim");
        actionOnCoolDown = true;
        wardrobeDoorIsOpen = false;
        yield return new WaitForSeconds(1.2f);
        actionOnCoolDown = false;
    }


    public void Interact(Interactor Interactor)
    {
        if(DayManager.instance.days == DayManager.Days.SUNDAY_MORNING && DayManager.instance.task == SundayMorning.SundayMorningTasks.LOOK_AROUND)
        {
            DialogueUIBehavior.instance.ShowDialogue(_wardrobeDialogue);
        }

        else if(DayManager.instance.days == DayManager.Days.MONDAY_NIGHT)
        {
            if (!wardrobeDoorIsOpen && !actionOnCoolDown)
            {
                //opens the door if the door is closed and there is no cooldown
                StartCoroutine(OpenWardrobeDoor());
               

            }
            else if (wardrobeDoorIsOpen && !actionOnCoolDown)
            {
                //Closes the door if the door is open and there is no cooldown
                StartCoroutine(CloseWardrobeDoor());
            }
        }
    }


    public void ToggleGetInOutOfWardrobe()
    {
        if(!playerIsInWardrobe)
        {
            _player.transform.position = _insideWardrobePos.transform.position;
            playerIsInWardrobe= true;

        }
        else if(playerIsInWardrobe)
        {
            _player.transform.position = _outsideWardrobePos.transform.position;
            playerIsInWardrobe= false;
        }
    }


    //Checks to see if the player is close enough to the wardrobe to get inside it and if the door is already open
    private void OnTriggerStay(Collider other)
    {
        if(wardrobeDoorIsOpen)
        {
            playerCanGetInWardrobe = true;
        }
        else
        {
            playerCanGetInWardrobe = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
       playerCanGetInWardrobe = false;
    }


    public void test()
    {
        Debug.Log("testing!");
        Debug.Log("YEP");
    }

    public void ResetPosition()
    {
        throw new System.NotImplementedException();
    }
}
