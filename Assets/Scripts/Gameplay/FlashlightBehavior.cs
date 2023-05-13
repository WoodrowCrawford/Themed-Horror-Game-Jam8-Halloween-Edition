using UnityEngine;


public class FlashlightBehavior : MonoBehaviour
{
    //Functions needed for this script to work
    public SleepBehavior sleepBehavior;
    public FlashlightTriggerBehavior flashlightTriggerBehavior;
    

    [Header("Flashlight Values")]
    [SerializeField]  private Light _playerLight; //The main flashlight light component
    [SerializeField] private float _decreaseSpeed;
    [SerializeField] private GameObject _flashlightTrigger; //The trigger box for the flashlight collider
    public float batteryPower = 100;

    public bool flashlightOn;
    



    private void Awake()
    {
        //Gets the components when awake is called
        sleepBehavior = GameObject.FindGameObjectWithTag("Player").GetComponent<SleepBehavior>();
        flashlightTriggerBehavior = GameObject.FindGameObjectWithTag("FlashlightTriggerBox").GetComponent<FlashlightTriggerBehavior>();
    }


    // Start is called before the first frame update
    void Start()
    {
        //Sets flashlight on to be true when the game starts
        flashlightOn = true;
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

            flashlightOn = false;
        }

       //if the flashlight is on...
        if(flashlightOn)
        {
            //Decrease the battery while the flashlight is on
            batteryPower -= (Time.deltaTime * _decreaseSpeed);
        
            //Turn off the flashlight if it reaches 0
            if(batteryPower <= 0)
            {
               ToggleFlashLight();
            }
        }

        //elsei f the flashlight is not on...
        else if(!flashlightOn)
        {
            //Increase battery while off
            batteryPower += Time.deltaTime * 9;
          
            //once the battery power is equal to or greater than 100...
            if (batteryPower >= 100f)
            {
                //Set the battery power to be 100
                batteryPower = 100f;
            }
        }
    }


    //Toggles the flashlight on and off
    public void ToggleFlashLight()
    {
        //If the game is not paused and the flash light is on..
        if(!PauseSystem.isPaused && flashlightOn)
        {
            _playerLight.gameObject.SetActive(false);

            //Set the flashlight collider to be off
            _flashlightTrigger.GetComponent<BoxCollider>().enabled = false;

            //sets the dummy is hit with light to be false when the light is off
            GameManager.instance.Dummy1.GetComponent<DummyStateManager>().dummyIsHitWithLight = false;
            GameManager.instance.Dummy2.GetComponent<DummyStateManager>().dummyIsHitWithLight = false;


            flashlightOn = false;
        }

        //if the game is not paused and the flashlight is off
        else if (!PauseSystem.isPaused && !flashlightOn)
        {
            _playerLight.gameObject.SetActive(true);

            //Set the flashlight collider to be off
            _flashlightTrigger.GetComponent<BoxCollider>().enabled = true;
            

            flashlightOn = true;
        }
    }
}
