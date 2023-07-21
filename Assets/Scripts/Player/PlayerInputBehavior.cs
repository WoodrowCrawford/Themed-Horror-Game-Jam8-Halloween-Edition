using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Timeline;


public class PlayerInputBehavior : MonoBehaviour
{
    //Important scripts
    public PlayerInputActions playerControls;                  //gets a script reference for the player input actions class
    public FlashlightBehavior flashlightBehavior;              //gets a script reference for the flashlight behavior class
    public GetInBedTriggerBehavior getInBedTriggerBehavior;    //gets a script reference for the get in the bed trigger class
    public SleepBehavior sleepBehavior;                        //gets a script referecne for the sleep behavior class
    public WardrobeBehavior wardrobeBehavior;                  //gets a script reference for the wardrobe behavior class
    public PauseSystem pauseSystem;                            //gets a script reference for the pause system class


    //Static bools used to determine if the player can do something or not
    [Header("Input bools")]
    public static bool playerCanPause = true;
    public static bool playerCanUseFlashlight = false;
    public static bool playerCanToggleUnderBed = true;
    public static bool playerCanGetInWardrobe = true;
    public static bool playerCanInteract = true;
    public static bool playerCanMove = true;
    public static bool playerCanLook = true;
    public static bool playerCanGetOutOfBed = true;
    public static bool playerCanGetInBed = true;
    public static bool playerCanSleep = false;
    

    [Header("Core Player Values")]
    public bool _playerIsHidden;




    //Used for interaction
    [Header("Interaction")]
    public static bool isPlayerInteracting = false;
    public static bool interactionWasPerfomed = false;

 

    //Camera stuff
    [Header("Camera Values")]
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _playerBody;
    [SerializeField] private Transform _startLookAt;
    [SerializeField] private float _sensitivity;
    [SerializeField] private float _xRotation = 0f;
    [SerializeField] private float _yRotation = 0f;
 
    //Movement stuff
    [Header("Movement Values")]
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed;


    

    [Header("Bed Values")]
    [SerializeField] private Transform _TopOfBedPos;
    [SerializeField] private Transform _UnderBedPos;
    [SerializeField] private Transform _outOfBedLeftPos;
    [SerializeField] private Transform _outOfBedRightPos;
    [SerializeField] public bool _isUnderBed = false;
    [SerializeField] public bool _inBed = false;


    [Header("Wardrobe Values")]
    [SerializeField] private Transform _WardrobeHidingPos;
    [SerializeField] private Transform _OutOfWardrobePos;
    public bool playerIsInWardrobe;
  

   
    public Camera Camera { get { return _camera; } }
  


    private void Awake()
    {
        //Gets the components
        _rb = GetComponent<Rigidbody>();
        flashlightBehavior = GameObject.FindGameObjectWithTag("Flashlight").GetComponent<FlashlightBehavior>();
        getInBedTriggerBehavior = GameObject.FindGameObjectWithTag("GetInBedTrigger").GetComponent<GetInBedTriggerBehavior>();
        sleepBehavior = GameObject.FindGameObjectWithTag("Player").GetComponent<SleepBehavior>();
        wardrobeBehavior = GameObject.FindGameObjectWithTag("Wardrobe").GetComponent<WardrobeBehavior>();
        pauseSystem = GameObject.FindGameObjectWithTag("PauseSystem").GetComponent<PauseSystem>(); 
    }

    public void OnEnable()
    {
        //Creates the Action Maps
        playerControls = new PlayerInputActions();
        playerControls.InBed.Enable();
        playerControls.Default.Enable();

        playerControls.Default.Interact.started += ctx => isPlayerInteracting = true;
        playerControls.Default.Interact.performed += ctx => interactionWasPerfomed = true;
        playerControls.Default.Interact.canceled += ctx => isPlayerInteracting = false;


        //Action Map #0(Default Map- On by default)
        playerControls.Default.Look.ReadValue<Vector2>();
        playerControls.Default.TogglePause.performed += ctx => pauseSystem.TogglePauseMenu();



        //Action Map #1 (In Bed)
        playerControls.InBed.GetOutOfBed.performed += GetOutOfBed;
        playerControls.InBed.ToggleFlashlight.performed += ctx => flashlightBehavior.ToggleFlashLight();
        playerControls.InBed.ToggleGoUnderBed.performed += ToggleUnderBed;
        playerControls.InBed.Sleep.performed += ctx => sleepBehavior.playerIsSleeping = true;
        playerControls.InBed.Sleep.canceled += ctx => sleepBehavior.playerIsSleeping = false;


        //Action Map #2 (Out of Bed)
        playerControls.OutOfBed.ToggleFlashlight.performed += ctx => flashlightBehavior.ToggleFlashLight();
        playerControls.OutOfBed.GetInBed.performed += GetInBed;



        //Action Map #3 (In Wardrobe)
        playerControls.InWardrobe.ToggleWardrobeDoor.performed += ctx => StartCoroutine(ToggleWardrobeDoor());
        playerControls.InWardrobe.ToggleInOutWardrobe.performed += ctx => StartCoroutine(ToggleInOutWardrobe());
        playerControls.InWardrobe.ToggleFlashlight.performed += ctx => flashlightBehavior.ToggleFlashLight();
    }

    public void OnDisable()
    {
        //Creates the Action Maps
        playerControls.Disable();
        playerControls.InBed.Disable();
        playerControls.Default.Disable();

        playerControls.Default.Interact.started -= ctx => isPlayerInteracting = true;
        playerControls.Default.Interact.performed -= ctx => isPlayerInteracting = true;
        playerControls.Default.Interact.canceled -= ctx => isPlayerInteracting = false;


        //Action Map #0(Default Map- On by default)
        playerControls.Default.Look.Disable();
        playerControls.Default.TogglePause.performed -= ctx => pauseSystem.TogglePauseMenu();



        //Action Map #1 (In Bed)
        playerControls.InBed.GetOutOfBed.performed -= GetOutOfBed;
        playerControls.InBed.ToggleFlashlight.performed -= ctx => flashlightBehavior.ToggleFlashLight();
        playerControls.InBed.ToggleGoUnderBed.performed -= ToggleUnderBed;
        playerControls.InBed.Sleep.performed -= ctx => sleepBehavior.playerIsSleeping = true;
        playerControls.InBed.Sleep.canceled -= ctx => sleepBehavior.playerIsSleeping = false;


        //Action Map #2 (Out of Bed)
        playerControls.OutOfBed.ToggleFlashlight.performed -= ctx => flashlightBehavior.ToggleFlashLight();
        playerControls.OutOfBed.GetInBed.performed -= GetInBed;



        //Action Map #3 (In Wardrobe)
        playerControls.InWardrobe.ToggleWardrobeDoor.performed -= ctx => StartCoroutine(ToggleWardrobeDoor());
        playerControls.InWardrobe.ToggleInOutWardrobe.performed -= ctx => StartCoroutine(ToggleInOutWardrobe());
        playerControls.InWardrobe.ToggleFlashlight.performed -= ctx => flashlightBehavior.ToggleFlashLight();
    }


    private void Start()
    {
        //Gets the player camera
        _camera = GetComponentInChildren<Camera>();

        //Hides the mouse
        Cursor.visible = false;

        //Makes the player look straight ahead when the game starts
        _camera.transform.LookAt(_startLookAt.position);

        //Sets the players position to be on top of the bed (can move this to be called in the day mananger for story reasons)
        _playerBody.transform.position = _TopOfBedPos.position;

        //sets the in bed variable to be true on startup
        _inBed = true;
    }

    private void Update()
    {


        _yRotation = Mathf.CeilToInt(_playerBody.transform.eulerAngles.y);

        //used for looking
        HandleLook();



        //If the player is under the bed then they should not be able to sleep
        if (_isUnderBed)
        {
            sleepBehavior.playerIsSleeping = false;
        }

        //Change the action map if the player is by the wardrobe
        if(wardrobeBehavior.playerCanOpenWardrobe)
        {
            playerControls.InWardrobe.Enable();
        }

        else if(!wardrobeBehavior.playerCanOpenWardrobe)
        {
            playerControls.InWardrobe.Disable();
        }
    }

    private void FixedUpdate()
    {
        //Move is in fixed update so that the move speed is consistant with any screen size
        HandleMove();   
    }

   



   ///////////Functions for both In Bed and out of bed/////////////////////////////////////////
  
    public void HandleLook()
    {
        //if the player can look...
        if(playerCanLook)
        {
            //if the dialogue box is open then return
            if (DialogueUIBehavior.IsOpen)
            {
                return;
            }


            //Look values
            float mouseXLook = playerControls.Default.Look.ReadValue<Vector2>().x * _sensitivity * Time.deltaTime;
            float mouseYLook = playerControls.Default.Look.ReadValue<Vector2>().y * _sensitivity * Time.deltaTime;


            _xRotation -= mouseYLook;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);


            _camera.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            _playerBody.Rotate(Vector3.up * mouseXLook);
        }

        return;
           
    }


    public void HandleMove()
    {
        if(playerCanMove)
        {
            //if the dialogue box is open then return
            if (DialogueUIBehavior.IsOpen)
            {
                return;
            }

            float x = playerControls.OutOfBed.Move.ReadValue<Vector2>().x;
            float y = playerControls.OutOfBed.Move.ReadValue<Vector2>().y;

            Vector3 move = transform.right * x + transform.forward * y;
            _rb.AddForce(move * _speed, ForceMode.Force);
        }

        return;
        
    }


    



    ////////////////Functions for Action Map #1 (In Bed)//////////////////////////////
    public void ToggleUnderBed(InputAction.CallbackContext context)
    {
        //if the player is allowed to toggle under the bed and the dialogue ui box is not open...
        if(playerCanToggleUnderBed && !DialogueUIBehavior.IsOpen)
        {
            if (!_isUnderBed && !sleepBehavior.playerIsSleeping && !PauseSystem.isPaused)
            {
                _playerBody.transform.position = _UnderBedPos.transform.position;
                _isUnderBed = true;
                _playerIsHidden = true;


            }
            else if (_isUnderBed && !PauseSystem.isPaused)
            {
                _playerBody.transform.position = _TopOfBedPos.transform.position;
                _isUnderBed = false;
                _playerIsHidden = false;
            }
        }

        return;
    }

    //Get out of the bed
    public void GetOutOfBed(InputAction.CallbackContext context)
    {
        //if the player is allowed to get out of the bed and the dialogue ui is not open...
        if (playerCanGetOutOfBed && !DialogueUIBehavior.IsOpen)
        {
            //check if the game is not paused...
            if (!PauseSystem.isPaused)
            {

                playerControls.InBed.Disable();
                playerControls.OutOfBed.Enable();

                //sets player is in bed bool to false
                _inBed = false;

                _playerIsHidden = false;


                if (_yRotation >= 180)
                {
                    _playerBody.transform.position = _outOfBedLeftPos.transform.position;
                }
                else if (_yRotation <= 180)
                {
                    _playerBody.transform.position = _outOfBedRightPos.transform.position;
                }
            }
        }

        return;
    }




//////////////////Functions for Action Map #2 (Out of Bed)////////////////////////
    public void GetInBed(InputAction.CallbackContext context)
    {
        //if the player is allowed to get in the bed and the dialogue ui is not open...
        if(playerCanGetInBed && !DialogueUIBehavior.IsOpen)
        {
            //check if the player is near the bed and make sure that the game is not paused
            if (getInBedTriggerBehavior.playerCanGetInBed && !PauseSystem.isPaused)
            {
                _playerBody.transform.position = _TopOfBedPos.position;
                playerControls.OutOfBed.Disable();
                playerControls.InBed.Enable();

                //sets the player in bed bool be true
                _inBed = true;

            }
        }

        return;
        
    }

    


//////////////Functions for Action Map #3 (In Wardrobe)//////////////////////////////////
    
    public IEnumerator ToggleWardrobeDoor()
    {
        //If the door is not open then open it
        if(!wardrobeBehavior.wardrobeDoorIsOpen && !wardrobeBehavior.actionOnCoolDown && !PauseSystem.isPaused)
        {
            StartCoroutine(wardrobeBehavior.OpenWardrobeDoor());
        }

        else if (wardrobeBehavior.wardrobeDoorIsOpen && !wardrobeBehavior.actionOnCoolDown && !PauseSystem.isPaused)
        {
            //else close the door
            StartCoroutine(wardrobeBehavior.CloseWardrobeDoor());
        }

        yield return null;
    }

    public IEnumerator ToggleInOutWardrobe()
    {
        if (wardrobeBehavior.wardrobeDoorIsOpen && !wardrobeBehavior.actionOnCoolDown && !playerIsInWardrobe && !PauseSystem.isPaused)
        {
            _playerBody.transform.position = _WardrobeHidingPos.transform.position;
            playerIsInWardrobe = true;
            _playerIsHidden = true;

            yield return new WaitForSeconds(1f);


            StartCoroutine(wardrobeBehavior.CloseWardrobeDoor());
        }

        else if (wardrobeBehavior.wardrobeDoorIsOpen && playerIsInWardrobe && !PauseSystem.isPaused)
        {   

            _playerBody.transform.position = _OutOfWardrobePos.transform.position;
            playerIsInWardrobe = false;
            _playerIsHidden = false;

            yield return new WaitForSeconds(0.5f);
            StartCoroutine(wardrobeBehavior.CloseWardrobeDoor());
        }
    }
   
}
