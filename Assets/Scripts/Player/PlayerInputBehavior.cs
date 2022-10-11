using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Timeline;
using UnityEngine.VFX;

public class PlayerInputBehavior : MonoBehaviour
{
    
    public PlayerInputActions playerControls;
    public FlashlightBehavior flashlightBehavior;
    public GetInBedTriggerBehavior getInBedTriggerBehavior;
    public SleepBehavior sleepBehavior;
    

    [Header("Camera Values")]
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _playerBody;
    [SerializeField] private float _sensitivity;
    [SerializeField] private float xRotation = 0f;
   

    [Header("Movement Values")]
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed;


    [Header("Bed Values")]
    [SerializeField] private Transform _TopOfBedPos;
    [SerializeField] private Transform _UnderBedPos;
    [SerializeField] private Transform _OutOfBedPos;
    public bool _isUnderBed = false;
    



    private void Awake()
    {
        //Gets the components
        _rb = GetComponent<Rigidbody>();
        flashlightBehavior = GameObject.FindGameObjectWithTag("Flashlight").GetComponent<FlashlightBehavior>();
        getInBedTriggerBehavior = GameObject.FindGameObjectWithTag("GetInBedTrigger").GetComponent<GetInBedTriggerBehavior>();
        sleepBehavior = GameObject.FindGameObjectWithTag("Player").GetComponent<SleepBehavior>();
     

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
    }

    private void Start()
    {
        //Gets the player camera
        _camera = GetComponentInChildren<Camera>();

        //Hides the mouse
        Cursor.visible = false;

        //Sets the players position to be on top of the bed
        _playerBody.transform.position = _TopOfBedPos.position;
    }

    private void Update()
    {
        Look();

        //If the player is under the bed then they should not be able to sleep
        if (_isUnderBed)
        {
            sleepBehavior.playerIsSleeping = false;
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
        //Look values for out of bed
        float mouseXinBed = playerControls.InBed.Look.ReadValue<Vector2>().x * _sensitivity * Time.deltaTime;
        float mouseYinBed = playerControls.InBed.Look.ReadValue<Vector2>().y * _sensitivity * Time.deltaTime;
        
        xRotation -= mouseYinBed;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        _camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        _playerBody.Rotate(Vector3.up * mouseXinBed);



        //Look values for out of bed
        float mouseXoutOfBed = playerControls.OutOfBed.Look.ReadValue<Vector2>().x * _sensitivity * Time.deltaTime;
        float mouseYoutOfBed = playerControls.OutOfBed.Look.ReadValue<Vector2>().y * _sensitivity * Time.deltaTime;

        xRotation -= mouseYoutOfBed;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        _camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
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

        _playerBody.transform.position = _OutOfBedPos.transform.position;
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
   
}
