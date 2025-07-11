using UnityEngine;


public class DoorBehavior : MonoBehaviour
{

    [Header("Animation Settings")]
    [SerializeField] private Animator _animator;



    [SerializeField] private string DOOR_OPEN = "DoorOpenAnim";
    [SerializeField] private string DOOR_CLOSE = "DoorCloseAnim";



    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<GhoulStateManager>())
        {
            
            _animator.Play(DOOR_OPEN, 0, 0.0f);

            SoundManager.instance.PlaySoundFXClip(SoundManager.instance.soundFXObject, SoundManager.instance.doorOpenClip, this.transform, false, 1f, 360f);

            //gameObject.SetActive(false);
        }  
    }


    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.GetComponent <GhoulStateManager>())
        {
           
            _animator.Play(DOOR_CLOSE, 0, 0.0f);

            SoundManager.instance.PlaySoundFXClip(SoundManager.instance.soundFXObject, SoundManager.instance.doorCloseClip, this.transform, false, 1f, 360f);
           
        }
       
    }

}
