using UnityEngine;

public class WallClockBehavior : MonoBehaviour
{
    private void Start()
    {
        SoundManager.instance.PlaySoundFXClipAtSetVolume(SoundManager.instance.soundFXObject, SoundManager.instance.tickingClockClip, this.transform, true, 1f, 1f);
    }
}
