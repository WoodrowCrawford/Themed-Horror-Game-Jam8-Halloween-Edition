using UnityEngine;

public class WallClockBehavior : MonoBehaviour
{
    private void OnEnable()
    {
        GameOverBehavior.onGameOver += StopClockSound;
        WinBehavior.onWin += StopClockSound;
    }

    

    private void OnDisable()
    {
        GameOverBehavior.onGameOver -= StopClockSound;
        WinBehavior.onWin -= StopClockSound;
    }


    private void Start()
    {
        SoundManager.instance.PlaySoundFXClipAtSetVolumeAndRange(SoundManager.instance.soundFXObject, SoundManager.instance.tickingClockClip, this.transform, true, 1f, 360f, 1f, 20f, 0.2f);
    }

    public void StopClockSound()
    {
        SoundManager.instance.StopSoundFXClip(SoundManager.instance.tickingClockClip);
    }
}
