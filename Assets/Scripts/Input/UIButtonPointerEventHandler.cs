using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonPointerEventHandler : MonoBehaviour, IPointerEnterHandler,  IPointerClickHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundFXManager.instance.PlaySoundFXClip(SoundFXManager.instance.buttonHoverClip, this.transform, false, 0f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
       if(gameObject.name == "BackButton")
        {
            //play back sound
            SoundFXManager.instance.PlaySoundFXClip(SoundFXManager.instance.backButtonClickClip, this.transform, false, 0f);
        }
        else
        {
            //play normal sound
            SoundFXManager.instance.PlaySoundFXClip(SoundFXManager.instance.buttonClickClip, this.transform, false, 0f);

        }
    }  
}
