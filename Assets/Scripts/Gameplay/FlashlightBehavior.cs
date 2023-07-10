using UnityEngine;


public class FlashlightBehavior : MonoBehaviour
{
    //Functions needed for this script to work
    public SleepBehavior sleepBehavior;
    public FlashlightTriggerBehavior flashlightTriggerBehavior;
    

    [Header("Flashlight Values")]
    [SerializeField] private bool _flashlightOn = true;
    [SerializeField] private float _batteryPower = 100;
    [SerializeField] private float _decreaseSpeed;
    [SerializeField] private Light _playerLight; //The main flashlight light component
    [SerializeField] private GameObject _flashlightTrigger; //The trigger box for the flashlight collider
   

    public GameObject flashlightGameObject; //The main game object that contains the flashlight components
    

   
    public bool FlashlightOn { get { return _flashlightOn; } set {  _flashlightOn = value; } }
    public float BatteryPower { get { return _batteryPower; } set { _batteryPower = value; } }
    public float DecreaseSpeed { get { return _decreaseSpeed; } set { _decreaseSpeed = value; } }



    private void Awake()
    {
        //Gets the components when awake is called
        sleepBehavior = GameObject.FindGameObjectWithTag("Player").GetComponent<SleepBehavior>();
        flashlightTriggerBehavior = GameObject.FindGameObjectWithTag("FlashlightTriggerBox").GetComponent<FlashlightTriggerBehavior>();
    }


   

    // Update is called once per frame
    void Update()
    {
        //If the player is sleeping then the flashlight should be off to prevent abuse of power
        if (sleepBehavior.playerIsSleeping)
        {
            //Sets the flashlight to be off
            _playerLight.gameObject.SetActive(false);

            //Set the flashlight collider to be off
            _flashlightTrigger.GetComponent<BoxCollider>().enabled = false;

            _flashlightOn = false;
        }

       //if the flashlight is on...
        if(_flashlightOn)
        {
            //Decrease the battery while the flashlight is on
            _batteryPower -= (Time.deltaTime * _decreaseSpeed);
        
            //Turn off the flashlight if it reaches 0
            if(_batteryPower <= 0)
            {
               ToggleFlashLight();
            }
        }

        //elsei f the flashlight is not on...
        else if(!_flashlightOn)
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



    //Toggles the flashlight on and off (used for player input)
    public void ToggleFlashLight()
    {
        //If the game is not paused and the flash light is on..
        if(!PauseSystem.isPaused && _flashlightOn && PlayerInputBehavior.playerCanUseFlashlight)
        {
            if(DialogueUIBehavior.IsOpen) { return; }

            _playerLight.gameObject.SetActive(false);

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

            _playerLight.gameObject.SetActive(true);

            //Set the flashlight collider to be off
            _flashlightTrigger.GetComponent<BoxCollider>().enabled = true;
            

            _flashlightOn = true;
        }
    }
}
