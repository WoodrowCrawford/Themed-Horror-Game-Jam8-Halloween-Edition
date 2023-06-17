using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Timeline;


public class PlayerInputBehavior : MonoBehaviour
{
    //Important scripts
    public PlayerInputActions playerControls;
    public FlashlightBehavior flashlightBehavior;
    public GetInBedTriggerBehavior getInBedTriggerBehavior;
    public SleepBehavior sleepBehavior;
    public WardrobeBehavior wardrobeBehavior;
    public PauseSystem pauseSystem;


    //Used for interaction
    [Header("Interaction")]
    public bool isPlayerInteracting;
    public static bool playerCanInteract = true;
   

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
    public bool _isUnderBed = false;
    public bool inBed = false;


    [Header("Wardrobe Values")]
    [SerializeField] private Transform _WardrobeHidingPos;
    [SerializeField] private Transform _OutOfWardrobePos;
    public bool playerIsInWardrobe;
    public bool playerIsHidden;


   

  


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
        playerControls.Default.Interact.performed += ctx => isPlayerInteracting = true;
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
    }

    private void Update()
    {
        _yRotation = Mathf.CeilToInt(_playerBody.transform.eulerAngles.y);

        Look();


        //If it is currently daytime...
        if(GraphicsBehavior.instance.IsDayTime)
        {
            //Cancel the sleeping and flashlight inputs
            playerControls.InBed.Sleep.Disable();
            playerControls.InBed.ToggleFlashlight.Disable();
            playerControls.OutOfBed.ToggleFlashlight.Disable();
            playerControls.InWardrobe.ToggleFlashlight.Disable();

            flashlightBehavior.TurnOffFlashlight();
           

        }
        else if(GraphicsBehavior.instance.IsNightTime)
        {
            //Cancel the sleeping and flashlight inputs
           
            playerControls.InBed.ToggleFlashlight.Enable();
            playerControls.OutOfBed.ToggleFlashlight.Enable();
            playerControls.InWardrobe.ToggleFlashlight.Enable();

           
        }


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
        Move();
    }

   


    //Functions for both In Bed and out of bed
    public void Look()
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


    public void Move()
    {
        //if the dialogue box is open then return
        if(DialogueUIBehavior.IsOpen)
        {
            return;
        }

        float x = playerControls.OutOfBed.Move.ReadValue<Vector2>().x;
        float y = playerControls.OutOfBed.Move.ReadValue<Vector2>().y;

        Vector3 move = transform.right * x + transform.forward * y;
        _rb.AddForce(move * _speed, ForceMode.Force);
    }


   



////////////////Functions for Action Map #1 (In Bed)//////////////////////////////
    public void ToggleUnderBed(InputAction.CallbackContext context)
    {
        if(!_isUnderBed && !sleepBehavior.playerIsSleeping && !PauseSystem.isPaused)
        {
            _playerBody.transform.position = _UnderBedPos.transform.position;
            _isUnderBed = true;
            playerIsHidden= true;
            
           
        }
        else if(_isUnderBed && !PauseSystem.isPaused)
        {
            _playerBody.transform.position = _TopOfBedPos.transform.position;
            _isUnderBed = false;
            playerIsHidden = false;
        }
    }

    //Get out of the bed
    public void GetOutOfBed(InputAction.CallbackContext context)
    {
       if(!PauseSystem.isPaused)
        {
            Debug.Log("I am out of bed!");
            

            playerControls.InBed.Disable();
            playerControls.OutOfBed.Enable();

            //sets player is in bed bool to false
            inBed = false;

            playerIsHidden = false;


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




//////////////////Functions for Action Map #2 (Out of Bed)////////////////////////
    public void GetInBed(InputAction.CallbackContext context)
    {
        if(getInBedTriggerBehavior.playerCanGetInBed && !PauseSystem.isPaused)
        {
            _playerBody.transform.position = _TopOfBedPos.position;
            playerControls.OutOfBed.Disable();
            playerControls.InBed.Enable();

            //sets the player in bed bool be true
            inBed = true;
            
        }
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
            playerIsHidden = true;

            yield return new WaitForSeconds(1f);


            StartCoroutine(wardrobeBehavior.CloseWardrobeDoor());
        }

        else if (wardrobeBehavior.wardrobeDoorIsOpen && playerIsInWardrobe && !PauseSystem.isPaused)
        {   

            _playerBody.transform.position = _OutOfWardrobePos.transform.position;
            playerIsInWardrobe = false;
            playerIsHidden = false;

            yield return new WaitForSeconds(0.5f);
            StartCoroutine(wardrobeBehavior.CloseWardrobeDoor());
        }
    }
   
}
