using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.iOS;
using UnityEngine.Timeline;


public class PlayerInputBehavior : MonoBehaviour
{
    
    public PlayerInputActions playerControls;
    public FlashlightBehavior flashlightBehavior;
    public GetInBedTriggerBehavior getInBedTriggerBehavior;
    public SleepBehavior sleepBehavior;
    public WardrobeBehavior wardrobeBehavior;
    public WardrobeDoorTriggerBehavior wardrobeDoorTrigger;
    

    [Header("Camera Values")]
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _playerBody;
    [SerializeField] private Transform _startLookAt;
    [SerializeField] private float _sensitivity;
    [SerializeField] private float _xRotation = 0f;
    [SerializeField] private float _yRotation = 0f;
 

    [Header("Movement Values")]
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed;


    [Header("Bed Values")]
    [SerializeField] private Transform _TopOfBedPos;
    [SerializeField] private Transform _UnderBedPos;
    [SerializeField] private Transform _outOfBedLeftPos;
    [SerializeField] private Transform _outOfBedRightPos;
    public bool _isUnderBed = false;


    [Header("Wardrobe Values")]
    [SerializeField] private Transform _WardrobeHidingPos;
    [SerializeField] private Transform _OutOfWardrobePos;

    public bool _playerIsInWardrobe;



    private void Awake()
    {
        //Gets the components
        _rb = GetComponent<Rigidbody>();
        flashlightBehavior = GameObject.FindGameObjectWithTag("Flashlight").GetComponent<FlashlightBehavior>();
        getInBedTriggerBehavior = GameObject.FindGameObjectWithTag("GetInBedTrigger").GetComponent<GetInBedTriggerBehavior>();
        sleepBehavior = GameObject.FindGameObjectWithTag("Player").GetComponent<SleepBehavior>();
        wardrobeBehavior = GameObject.FindGameObjectWithTag("Wardrobe").GetComponent<WardrobeBehavior>();
        wardrobeDoorTrigger = GameObject.FindGameObjectWithTag("WardrobeDoorTrigger").GetComponent<WardrobeDoorTriggerBehavior>();
     

        //Creates the Action Maps
        playerControls = new PlayerInputActions();
        playerControls.InBed.Enable();


        //Action Map #1 (In Bed)
        playerControls.InBed.GetOutOfBed.performed += GetOutOfBed;
        playerControls.InBed.Look.ReadValue<Vector2>();
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
        

    }

    private void Start()
    {
        //Gets the player camera
        _camera = GetComponentInChildren<Camera>();


        //Hides the mouse
        Cursor.visible = false;

        //Makes the player look straight ahead when the game starts
        _camera.transform.LookAt(_startLookAt.position);


        //Sets the players position to be on top of the bed
        _playerBody.transform.position = _TopOfBedPos.position;
   
    }

    private void Update()
    {
      _yRotation = Mathf.CeilToInt(_playerBody.transform.eulerAngles.y);
        Look();

        //If the player is under the bed then they should not be able to sleep
        if (_isUnderBed)
        {
            sleepBehavior.playerIsSleeping = false;
        }

        //Change the action map if the player is by the wardrobe
        if(wardrobeDoorTrigger.playerCanOpenWardrobe)
        {
            playerControls.InWardrobe.Enable();
        }

        else if(!wardrobeDoorTrigger.playerCanOpenWardrobe)
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
        //Look values for in the bed
        float mouseXinBed = playerControls.InBed.Look.ReadValue<Vector2>().x * _sensitivity * Time.deltaTime;
        float mouseYinBed = playerControls.InBed.Look.ReadValue<Vector2>().y * _sensitivity * Time.deltaTime;
       

        _xRotation -= mouseYinBed;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);


        _camera.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        _playerBody.Rotate(Vector3.up * mouseXinBed);

        ////////////////////////////////////////////////////////////////////////////////

        //Look values for out of bed
        float mouseXoutOfBed = playerControls.OutOfBed.Look.ReadValue<Vector2>().x * _sensitivity * Time.deltaTime;
        float mouseYoutOfBed = playerControls.OutOfBed.Look.ReadValue<Vector2>().y * _sensitivity * Time.deltaTime;

        _xRotation -= mouseYoutOfBed;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        _camera.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        _playerBody.Rotate(Vector3.up * mouseXoutOfBed);
    }

    public void Move()
    {
        float x = playerControls.OutOfBed.Move.ReadValue<Vector2>().x;
        float y = playerControls.OutOfBed.Move.ReadValue<Vector2>().y;

        Vector3 move = transform.right * x + transform.forward * y;
        _rb.AddForce(move * _speed, ForceMode.Force);
    }




    //Functions for Action Map #1 (In Bed)
    public void ToggleUnderBed(InputAction.CallbackContext context)
    {
        if(!_isUnderBed && !sleepBehavior.playerIsSleeping)
        {
            _playerBody.transform.position = _UnderBedPos.transform.position;
            _isUnderBed = true;
           
        }
        else if(_isUnderBed)
        {
            _playerBody.transform.position = _TopOfBedPos.transform.position;
            _isUnderBed = false;
        }
    }

    //Get out of the bed
    public void GetOutOfBed(InputAction.CallbackContext context)
    {
        Debug.Log("I am out of bed!");
        playerControls.InBed.Disable();
        playerControls.OutOfBed.Enable();

        if(_yRotation >= 180)
        {
            _playerBody.transform.position = _outOfBedLeftPos.transform.position;
        }
        else if (_yRotation <= 180)
        {
            _playerBody.transform.position = _outOfBedRightPos.transform.position;
        } 
    }



    //Functions for Action Map #2 (Out of Bed)
    public void GetInBed(InputAction.CallbackContext context)
    {
        if(getInBedTriggerBehavior.playerCanGetInBed)
        {
            _playerBody.transform.position = _TopOfBedPos.position;
            playerControls.OutOfBed.Disable();
            playerControls.InBed.Enable();
        }
    }


    //Functions for Action Map #3 (In Wardrobe)
    
    public IEnumerator ToggleWardrobeDoor()
    {
        //If the door is not open then open it
        if(!wardrobeBehavior.wardrobeDoorIsOpen && !wardrobeBehavior.actionOnCoolDown)
        {
            StartCoroutine(wardrobeBehavior.OpenWardrobeDoor());
        }

        else if (wardrobeBehavior.wardrobeDoorIsOpen && !wardrobeBehavior.actionOnCoolDown)
        {
            StartCoroutine(wardrobeBehavior.CloseWardrobeDoor());
        }

        yield return null;
    }

    public IEnumerator ToggleInOutWardrobe()
    {
        if(wardrobeBehavior.wardrobeDoorIsOpen && !wardrobeBehavior.actionOnCoolDown && !_playerIsInWardrobe)
        {
            _playerBody.transform.position = _WardrobeHidingPos.transform.position;
            _playerIsInWardrobe = true;
            yield return new WaitForSeconds(1f);
            

            StartCoroutine(wardrobeBehavior.CloseWardrobeDoor());
        }

       else if(wardrobeBehavior.wardrobeDoorIsOpen && _playerIsInWardrobe)
        {

            _playerBody.transform.position = _OutOfWardrobePos.transform.position;
            _playerIsInWardrobe = false;

            yield return new WaitForSeconds(0.5f);
            StartCoroutine(wardrobeBehavior.CloseWardrobeDoor());
        }
    }
   
}
