using UnityEngine;


public class FlashlightBehavior : MonoBehaviour
{ 

    [Header("Flashlight Values")]
    [SerializeField] private bool _flashlightOn = false;
    public static bool flashlightCanDeplete = false;
    [SerializeField] private float _batteryPower = 100;
    [SerializeField] private float _decreaseSpeed;
    [SerializeField] private Light _playerLight; //The main flashlight light component
    [SerializeField] private GameObject _flashlightTrigger; //The trigger box for the flashlight collider
   

    public GameObject flashlightGameObject; //The main game object that contains the flashlight components


    
    public bool FlashlightOn { get { return _flashlightOn; } set {  _flashlightOn = value; } }
    public float BatteryPower { get { return _batteryPower; } set { _batteryPower = value; } }
    public float DecreaseSpeed { get { return _decreaseSpeed; } set { _decreaseSpeed = value; } }





    private void OnEnable()
    {
       
        PlayerInputBehavior.onFlashlightButtonPressed += ToggleFlashLight;
        WindowStateManager.onWindowOpened += SetFlashlightDecreaseSpeedTo25;
        WindowStateManager.onWindowClosed += SetFlashlightDecreaseSpeedTo9;

        DayManager.OnDayTime += SetFlashlightCanBeUsedToFalse;
        DayManager.OnNightTime += SetFlashlightCanBeUsedToTrue;

        EyesBehavior.onPlayerEyesAreFullyOpen += SetFlashlightCanBeUsedToTrue;

       
      
    }

    private void OnDisable()
    {
     
        PlayerInputBehavior.onFlashlightButtonPressed -= ToggleFlashLight;
        WindowStateManager.onWindowOpened -= SetFlashlightDecreaseSpeedTo25;
        WindowStateManager.onWindowClosed -= SetFlashlightDecreaseSpeedTo9;

        DayManager.OnDayTime -= SetFlashlightCanBeUsedToFalse;
        DayManager.OnNightTime -= SetFlashlightCanBeUsedToTrue;

        EyesBehavior.onPlayerEyesAreFullyOpen -= SetFlashlightCanBeUsedToTrue;

    }



   


    private void Start()
    {
       
        //if it is daytime...
        if (GraphicsBehavior.instance.IsDayTime)
        {
            //set to be false
            _flashlightOn = false;
        }
        else
        {
            //set to be true
            _flashlightOn = true;

        }
    }


    // Update is called once per frame
    void Update()
    {
        if (!PauseSystem.isPaused)
        {
            //checks to see if the player is sleeping
            CheckIfPlayerIsSleeping();

            //handles the battery power for the flashlight
            HandleFlashlightBatteryPower();
        }
        else
        {
            return;
        }
        
    }

  

    public void CheckIfPlayerIsSleeping()
    {
        //If the player is sleeping then the flashlight should be off to prevent abuse of power
        if (SleepBehavior.playerIsSleeping)
        {
            //Sets the flashlight to be off
            _playerLight.GetComponent<Light>().enabled = false;

            //Set the flashlight collider to be off
            _flashlightTrigger.GetComponent<BoxCollider>().enabled = false;

            _flashlightOn = false;
        }
    }

   

    public void SetFlashlightDecreaseSpeedTo25()
    {
        _decreaseSpeed = 25f;
    }

    public void SetFlashlightDecreaseSpeedTo9()
    {
        _decreaseSpeed = 9f;
    }



    public void HandleFlashlightBatteryPower()
    {
        //if the flashlight is on and the battery is able to deplete...
        if (_flashlightOn && flashlightCanDeplete)
        {
            //Decrease the battery while the flashlight is on
            _batteryPower -= (Time.deltaTime * _decreaseSpeed);

            //if the flashlight power is less than or equal to 0...
            if (_batteryPower <= 0)
            {
                //toggle the flashlight (turns it off in this case)
                ToggleFlashLight();
            }
        }

        //else if the flashlight is not on...
        else if (!_flashlightOn)
        {
            //Increase battery while off
            _batteryPower += Time.deltaTime * 9;

            //once the battery power is equal to or greater than 100...
            if (_batteryPower >= 100f)
            {
                //Set the battery power to be 100
                _batteryPower = 100f;
            }
        }
    }

  


    public void SetFlashlightCanBeUsedToTrue()
    {
        PlayerInputBehavior.playerCanUseFlashlight = true;
    }


    public void SetFlashlightCanBeUsedToFalse()
    {
        PlayerInputBehavior.playerCanUseFlashlight = false;
        TurnOffFlashlight();
    }


    public void TurnOnFlashlight()
    {
        _playerLight.gameObject.SetActive(true);

        //Set the flashlight collider to be off
        _flashlightTrigger.GetComponent<BoxCollider>().enabled = true;


        _flashlightOn = true;
    }

    public void TurnOffFlashlight()
    {
        _playerLight.gameObject.SetActive(false);

        //Set the flashlight collider to be off
        _flashlightTrigger.GetComponent<BoxCollider>().enabled = false;

        _flashlightOn = false;
    }



    //Toggles the flashlight on and off (used for player input)
    public void ToggleFlashLight()
    {


        //If the game is not paused and the flash light is on..
        if(!PauseSystem.isPaused && _flashlightOn && PlayerInputBehavior.playerCanUseFlashlight)
        {
            if(DialogueUIBehavior.IsOpen) { return; }

            //play turn off sound
            SoundManager.instance.PlaySoundFXClipAtSetVolume(SoundManager.instance.soundFXObject, SoundManager.instance.flashlightClickOffClip, this.transform, false, 0f, 0f, 0.009f);

            //turn off the flashlight component
            _playerLight.GetComponent<Light>().enabled = false;

            

            //Set the flashlight collider to be off
            _flashlightTrigger.GetComponent<BoxCollider>().enabled = false;

            //sets the dummy is hit with light to be false when the light is off (Fix to be called with dummy instead)
            DayManager.instance.Dummy1.GetComponent<DummyStateManager>().dummyIsHitWithLight = false;
            DayManager.instance.Dummy2.GetComponent<DummyStateManager>().dummyIsHitWithLight = false;
            

            _flashlightOn = false;
        }

        //if the game is not paused and the flashlight is off
        else if (!PauseSystem.isPaused && !_flashlightOn && PlayerInputBehavior.playerCanUseFlashlight)
        {
            if (DialogueUIBehavior.IsOpen) { return; }

            //play turn on sound
            SoundManager.instance.PlaySoundFXClipAtSetVolume(SoundManager.instance.soundFXObject, SoundManager.instance.flashlightClickOnClip, this.transform, false, 0f, 0f, 0.009f);

            //turn on the flashlight component
            _playerLight.GetComponent<Light>().enabled = true;

           

            //Set the flashlight collider to be off
            _flashlightTrigger.GetComponent<BoxCollider>().enabled = true;
            

            _flashlightOn = true;
        }
        
        else if (!PlayerInputBehavior.playerCanUseFlashlight)
        {
            Debug.Log("The player is not allowed to use flashlight");
        }
        
       
    }


    
}
