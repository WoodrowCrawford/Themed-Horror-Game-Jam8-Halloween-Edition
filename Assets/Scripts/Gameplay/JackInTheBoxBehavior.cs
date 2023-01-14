using UnityEngine;
using UnityEngine.InputSystem;


public class JackInTheBoxBehavior : MonoBehaviour
{
    public Animator _animator;
    public GameObject _handle;

    [Header("Jack In the Box Values")]
    [SerializeField] private float _musicDuration = 100;
    [SerializeField] private float _decreaseSpeed;
    [SerializeField] private float _increaseSpeed;
    [SerializeField] private bool _playerRewindingBox;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _musicDuration= 100;
        _playerRewindingBox = false;
        _animator.enabled = false;
       
    }

    private void Update()
    {
       

        if(_playerRewindingBox)
        {
            RewindMusicBox();
        }
        else
        {
            PlayMusicBox();
        }



        if(_musicDuration <= 0f)
        {
            _handle.gameObject.transform.Rotate(0, 0, 0);
        }
        else if (_musicDuration >= 100f)
        {
            _handle.gameObject.transform.Rotate(0, 0, 0);
        }

    }



    public void PlayMusicBox()
    {
      
        //decrease the music duration as time passes
       _musicDuration -= (Time.deltaTime * _decreaseSpeed);
        
        //sets the music duration to be 0 if equals 0 or less
        if (_musicDuration<= 0f)
        {
            _musicDuration= 0f;
        }

        
        //Rotates the handle while the music is playing
        _handle.gameObject.transform.Rotate(1, 0, 0);
    }


    public void RewindMusicBox()
    {
        //Increase the music duration as the box is rewinding
        _musicDuration += (Time.deltaTime * _increaseSpeed);

        //Rotates the handle the opposite direction while rewinding
        _handle.gameObject.transform.Rotate(-1, 0, 0);
    }

}





