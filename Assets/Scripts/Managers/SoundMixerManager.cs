using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    public static SoundMixerManager instance;

    [SerializeField] private AudioMixer audioMixer;
    


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public float GetMasterVolume()
    {
        audioMixer.GetFloat("masterVolume", out float masterVolume);
        return Mathf.Pow(10f, masterVolume / 20f);
    }

    public float GetSoundFXVolume()
    {
        audioMixer.GetFloat("soundFXVolume", out float soundFXVolume);
        return Mathf.Pow(10f, soundFXVolume / 20f);
    }

    public float GetMusicVolume()
    {
        audioMixer.GetFloat("musicVolume", out float musicVolume);
        return Mathf.Pow(10f, musicVolume / 20f);
    }


    public void SetMasterVolume(float level)
    {
     
        audioMixer.SetFloat("masterVolume", Mathf.Log10(level) * 20f);
    }


    public void SetSoundFXVolume(float level)
    {
        audioMixer.SetFloat("soundFXVolume", Mathf.Log10(level) * 20f);
    }


    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20f);
    }
}
