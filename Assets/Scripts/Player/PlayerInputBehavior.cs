using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;



public class PlayerInputBehavior : MonoBehaviour
{
    //Important scripts
    public PlayerInputActions playerControls;                  //gets a script reference for the player input actions class
    
 
    public CinemachineCamera Camera { get { return _camera; } }

    [SerializeField] private InputActionMap _currentActionMap { get; set; }
    

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
    public static bool playerCanGetCaught = true;
    


    //Delegates
    public delegate void PlayerInputEventHandler();
   



    //Interaction events
    public static event PlayerInputEventHandler onPlayerIsInteracting;
    public static event PlayerInputEventHandler onInteractionWasPerformed;

    //Pause events
    public static event PlayerInputEventHandler onPausedButtonPressed;
    public static event PlayerInputEventHandler onGetOutOfBedButtonPressed;

    //Flashlight events
    public static event PlayerInputEventHandler onFlashlightButtonPressed;

    //Sleep events
    public static event PlayerInputEventHandler onSleepButtonPressed;
    public static event PlayerInputEventHandler onSleepButtonReleased;

    //Wardrobe events
    public static event PlayerInputEventHandler onWardrobeInteractButtonPressed;



    [Header("Core Player Values")]
    [SerializeField] private GameObject _playerObject;
    [SerializeField] private bool _playerIsHidden;

    //Used for interaction
    [Header("Interaction")]
    public static bool isPlayerInteracting = false;
    public static bool interactionWasPerfomed = false;
    public bool playerIsInWardrobe;


    //Camera stuff
    [Header("Camera Values")]
    //[SerializeField] private Camera _camera;
    [SerializeField] private CinemachineCamera _camera;
    [SerializeField] private InputAxis _inputAxisState;
    [SerializeField] private Transform _playerBodyTransform;
    [SerializeField] private Transform _startLookAt;
    public static float sensitivity = 20f;
   
    [SerializeField] private float _xRotation = 0f;
    [SerializeField] private float _yRotation = 0f;



    [Header("Cinemachine Pov Camera")]
    [SerializeField] private float _povXRotation;
    [SerializeField] private float _povYRotation;


    //Movement stuff
    [Header("Movement Values")]
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed;


    

    [Header("Bed Values")]
    [SerializeField] private Transform _TopOfBedPos;
    [SerializeField] private Transform _UnderBedPos;
    [SerializeField] private Transform _outOfBedLeftPos;
    [SerializeField] private Transform _outOfBedRightPos;
    [SerializeField] private bool _isUnderBed = false;
    [SerializeField] public static bool inBed = false;


   
   
  
    
   public bool PlayerIsHidden { get { return _playerIsHidden; } set { _playerIsHidden = value; } }
    public Transform PlayerBody {  get { return _playerBodyTransform; } set { _playerBodyTransform = value; } }
    public Transform TopOfBedPos { get { return _TopOfBedPos; } set { _TopOfBedPos = value; } }
    public Transform OutOfBedLeftPos { get { return _outOfBedLeftPos; } set { _outOfBedLeftPos = value; } }
    public bool IsUnderBed { get { return _isUnderBed; }  set { _isUnderBed = value; } }
   
  


   
    public void OnEnable()
    {
        //Subscribes to the events
        onPlayerIsInteracting += () => isPlayerInteracting = true;
        onInteractionWasPerformed += () => interactionWasPerfomed = true;
        onGetOutOfBedButtonPressed += GetOutOfBed;

    
        SleepBehavior.onPlayerCloseEyes += () => DisableControlsWhileSleeping();
        SleepBehavior.onPlayerOpenEyes += () => EnableControlsWhileWakingUp();

      
        //Creates the Action Maps
        playerControls = new PlayerInputActions();
        playerControls.InBed.Enable();
        playerControls.Default.Enable();

        playerControls.Default.Interact.started += ctx => onPlayerIsInteracting?.Invoke();
        playerControls.Default.Interact.performed += ctx => onInteractionWasPerformed?.Invoke();
        playerControls.Default.Interact.canceled += ctx => isPlayerInteracting = false;


        //Action Map #0(Default Map- On by default)
        playerControls.Default.Look.ReadValue<Vector2>();
        playerControls.Default.TogglePause.performed += ctx => onPausedButtonPressed?.Invoke();



        //Action Map #1 (In Bed)
        playerControls.InBed.GetOutOfBed.performed +=  ctx => onGetOutOfBedButtonPressed?.Invoke();
        playerControls.InBed.ToggleFlashlight.performed += ctx => onFlashlightButtonPressed?.Invoke();                  
        playerControls.InBed.ToggleGoUnderBed.performed += ToggleUnderBed;

        playerControls.InBed.Sleep.performed += ctx => onSleepButtonPressed?.Invoke();
        playerControls.InBed.Sleep.canceled += ctx => onSleepButtonReleased?.Invoke();


        //Action Map #2 (Out of Bed)
        playerControls.OutOfBed.ToggleFlashlight.performed += ctx => onFlashlightButtonPressed?.Invoke();
        playerControls.OutOfBed.GetInBed.performed += GetInBed;



        //Action Map #3 (In Wardrobe)
        playerControls.InWardrobe.ToggleWardrobeDoor.performed += ctx => onWardrobeInteractButtonPressed?.Invoke();
        playerControls.InWardrobe.ToggleInOutWardrobe.performed += ctx => onWardrobeInteractButtonPressed?.Invoke();
        playerControls.InWardrobe.ToggleFlashlight.performed += ctx => onFlashlightButtonPressed?.Invoke();
    }

    public void OnDisable()
    {
        //Unsubscribes to the events
        onPlayerIsInteracting -= () => isPlayerInteracting = true;
        onInteractionWasPerformed -= () => interactionWasPerfomed = true;
        onGetOutOfBedButtonPressed -= GetOutOfBed;
    
        SleepBehavior.onPlayerCloseEyes -= () => DisableControlsWhileSleeping();
        SleepBehavior.onPlayerOpenEyes -= () => EnableControlsWhileWakingUp();


        //Creates the Action Maps
        playerControls.Disable();
        playerControls.InBed.Disable();
        playerControls.Default.Disable();

        playerControls.Default.Interact.started -= ctx => onPlayerIsInteracting?.Invoke();
        playerControls.Default.Interact.performed -= ctx => onInteractionWasPerformed?.Invoke();
        playerControls.Default.Interact.canceled -= ctx => isPlayerInteracting = false;


        //Action Map #0(Default Map- On by default)
        playerControls.Default.Look.Disable();
        playerControls.Default.TogglePause.performed -= ctx => onPausedButtonPressed?.Invoke();



        //Action Map #1 (In Bed)
        playerControls.InBed.GetOutOfBed.performed -= ctx => onGetOutOfBedButtonPressed?.Invoke();
        playerControls.InBed.ToggleFlashlight.performed -= ctx => onFlashlightButtonPressed?.Invoke();
        playerControls.InBed.ToggleGoUnderBed.performed -= ToggleUnderBed;
        playerControls.InBed.Sleep.performed -= ctx => onSleepButtonPressed?.Invoke();
        playerControls.InBed.Sleep.canceled -= ctx => onSleepButtonReleased?.Invoke();


        //Action Map #2 (Out of Bed)
        playerControls.OutOfBed.ToggleFlashlight.performed -= ctx => onFlashlightButtonPressed?.Invoke();
        playerControls.OutOfBed.GetInBed.performed -= GetInBed;



        //Action Map #3 (In Wardrobe)
        playerControls.InWardrobe.ToggleWardrobeDoor.performed -= ctx => onWardrobeInteractButtonPressed?.Invoke();
        playerControls.InWardrobe.ToggleInOutWardrobe.performed -= ctx => onWardrobeInteractButtonPressed?.Invoke();
        playerControls.InWardrobe.ToggleFlashlight.performed -= ctx => onFlashlightButtonPressed?.Invoke();
    }



    private void Awake()
    {
        //Gets the components
        _playerObject = GetComponent<PlayerInputBehavior>().gameObject;
        _rb = GetComponent<Rigidbody>();     

        //Gets the player camera
        _camera = GameObject.Find("Player VCam").GetComponent<CinemachineCamera>();
    }


   

   


    private void Start()
    {
        
        //Hides the mouse
        Cursor.visible = false;

        //Makes the player look straight ahead when the game starts
        _camera.transform.LookAt(_startLookAt.position);

        //Sets the players position to be on top of the bed (can move this to be called in the day mananger for story reasons)
        _playerBodyTransform.transform.position = _TopOfBedPos.position;

        //sets the current action map to be in bed on startup
        _currentActionMap = playerControls.InBed;

        //sets the in bed variable to be true on startup
        inBed = true;

        //set to be true on startup
        playerCanGetCaught = true;

        //enable controls on startup
        EnableControls();

        
    }

    private void Update()
    {
        Debug.Log(_currentActionMap);

        
        _yRotation = Mathf.CeilToInt(_playerBodyTransform.transform.eulerAngles.y);

        if (playerCanLook)
            HandleLook();

       
    }

    private void FixedUpdate()
    {
        //Move is in fixed update so that the move speed is consistant with any screen size
        HandleMove();   
    }




    public void DisableControls()
    {
        playerControls.Disable();
    }

    public void DisableControlsWhileSleeping()
    {
        playerCanUseFlashlight = false;
        playerCanLook = false;
        playerCanGetOutOfBed = false;
        playerCanPause = false;
        playerCanMove = false;
        playerCanToggleUnderBed = false;
    }

    //Enables the controls
    public void EnableControls()
    {
        playerControls.Default.Enable();
        _currentActionMap.Enable();
    }

    public void EnableControlsWhileWakingUp()
    {
        playerCanUseFlashlight = true;
        playerCanLook = true;
         playerCanGetOutOfBed = true;
        playerCanPause = true;
        playerCanMove = true;
        playerCanToggleUnderBed = true;
    }

    public void HidePlayer()
    {
        _playerObject.GetComponent<MeshRenderer>().enabled = false;
    }



    public void SetPlayerCanGetCaught(bool canGetCaught)
    {
        playerCanGetCaught = canGetCaught;
    }





    ///////////Functions for both In Bed and out of bed/////////////////////////////////////////

    public void HandleLook()
    {
        //if the dialogue box is open then return
        if (DialogueUIBehavior.IsOpen)
        {
            return;
        }

       
       
        


        ////Look values
        float mouseXLook = playerControls.Default.Look.ReadValue<Vector2>().x * sensitivity  * Time.deltaTime;
        float mouseYLook = playerControls.Default.Look.ReadValue<Vector2>().y * sensitivity * Time.deltaTime;


        _xRotation -= mouseYLook;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);


        _camera.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        _playerBodyTransform.Rotate(Vector3.up * mouseXLook);
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
            ///if the player is not under the bed and the player is not sleeping and the game is not paused
            if (!_isUnderBed && !SleepBehavior.playerIsSleeping && !PauseSystem.isPaused)
            {
                //set the player to be under the bed
                _playerBodyTransform.transform.position = _UnderBedPos.transform.position;

                //set isUnderBed to be true
                _isUnderBed = true;

                //Set player is hidden to be true
                _playerIsHidden = true;

                //the player can not sleep while under the bed
                playerCanSleep = false;

                //the player can not get out of bed while under it
                playerCanGetOutOfBed = false;


            }
            else if (_isUnderBed && !PauseSystem.isPaused)
            {
                //set the player to be on top of the bed
                _playerBodyTransform.transform.position = _TopOfBedPos.transform.position;

                //set isUnderBed to be false
                _isUnderBed = false;

                //set player is hidden to be false
                _playerIsHidden = false;

                //check if it is nighttime
                if(GraphicsBehavior.instance.IsNightTime)
                {
                    //the player is allowed to sleep
                    playerCanSleep = true;
                }

                playerCanGetOutOfBed = true;
            }
        }

        return;
    }

    //Get out of the bed
    public void GetOutOfBed()
    {
        //if the player is allowed to get out of the bed and the dialogue ui is not open...
        if (playerCanGetOutOfBed && !DialogueUIBehavior.IsOpen)
        {
            //check if the game is not paused...
            if (!PauseSystem.isPaused)
            {

                playerControls.InBed.Disable();


                playerControls.OutOfBed.Enable();
                _currentActionMap = playerControls.OutOfBed;

                //sets player is in bed bool to false
                inBed = false;

                _playerIsHidden = false;


                if (_yRotation >= 180)
                {
                    _playerBodyTransform.transform.position = _outOfBedLeftPos.transform.position;
                }
                else if (_yRotation <= 180)
                {
                    _playerBodyTransform.transform.position = _outOfBedRightPos.transform.position;
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
            if (GetInBedTriggerBehavior.playerCanGetInBed && !PauseSystem.isPaused)
            {
                _playerBodyTransform.transform.position = _TopOfBedPos.position;
                playerControls.OutOfBed.Disable();

                playerControls.InBed.Enable();
                _currentActionMap = playerControls.InBed;

                //sets the player in bed bool be true
                inBed = true;

            }
        }

        return;
        
    }
}
