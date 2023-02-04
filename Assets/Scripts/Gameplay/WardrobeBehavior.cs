using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;

public class WardrobeBehavior : MonoBehaviour, IInteractable
{
  
    public PlayerInputBehavior playerInputBehavior;
    public Animator animator;

    [SerializeField] private string _interactionPrompt;

    [SerializeField] private GameObject _wardrobeDoorTrigger;
    [SerializeField] private GameObject _player;

    [Header("Wardrobe Positions")]
    [SerializeField] private Transform _insideWardrobePos;
    [SerializeField] private Transform _outsideWardrobePos;



    public bool actionOnCoolDown = false;
    public bool playerCanOpenWardrobe;
    public bool wardrobeDoorIsOpen = false;
    public bool playerCanGetInWardrobe = false;

    public bool playerIsInWardrobe = false;


    public string InteractionPrompt => _interactionPrompt;

    private void Awake()
    {
        playerInputBehavior = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputBehavior>();
    }


    private void Update()
    {
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
}
