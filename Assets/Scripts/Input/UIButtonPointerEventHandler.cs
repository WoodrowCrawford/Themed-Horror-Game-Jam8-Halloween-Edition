using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonPointerEventHandler : MonoBehaviour, IPointerEnterHandler,  IPointerClickHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.instance.PlaySoundFXClipAtSetVolume(SoundManager.instance.uiSoundObject, SoundManager.instance.buttonHoverClip, this.transform, false, 0f, 0.2f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
       if(gameObject.name == "BackButton")
        {
            //play back sound
            SoundManager.instance.PlaySoundFXClipAtSetVolume(SoundManager.instance.uiSoundObject, SoundManager.instance.backButtonClickClip, this.transform, false, 0f, 0.2f);
        }
        else
        {
            //play normal sound
            SoundManager.instance.PlaySoundFXClipAtSetVolume(SoundManager.instance.uiSoundObject, SoundManager.instance.buttonClickClip, this.transform, false, 0f, 0.2f);

        }
    }  
}
