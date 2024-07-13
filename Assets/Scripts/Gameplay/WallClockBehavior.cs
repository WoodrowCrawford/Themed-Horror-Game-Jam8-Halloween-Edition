using UnityEngine;

public class WallClockBehavior : MonoBehaviour
{
    private void Start()
    {
        SoundFXManager.instance.PlaySoundFXClipAtSetVolume(SoundFXManager.instance.tickingClockClip, this.transform, true, 1f, 0.8f);
    }
}
