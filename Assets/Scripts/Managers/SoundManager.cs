using UnityEngine;



//A script that controls the creation on sounds in the game with different sound types

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Sound types")]
    public AudioSource soundFXObject;
    public AudioSource uiSoundObject;

    [Header("UI Clips")]
    public AudioClip buttonHoverClip;
    public AudioClip buttonClickClip;
    public AudioClip backButtonClickClip;
    public AudioClip UpdateTaskClip;


    [Header("Door Clips")]
    public AudioClip doorOpenClip;
    public AudioClip doorCloseClip;

    [Header("Wall Clock Clips")]
    public AudioClip tickingClockClip;

    [Header("Window Clips")]
    public AudioClip windowOpeningStartUpClip;
    public AudioClip windowOpeningContinuousClip;


    [Header("Flashlight Clips")]
    public AudioClip flashlightClickOnClip;
    public AudioClip flashlightClickOffClip;

    [Header("Dummy Sound Clips")]
    public AudioClip dummyGetUpClip;
    public AudioClip dummyJumpScareClip;

    [Header("Dummy Footstep Clips")]
    public AudioClip[] dummyFootsteps;

    [Header("Ghoul Sound Clips")]
    public AudioClip ghoulJumpScareClip;

    [Header("Ghoul Footstep Clips")]
    public AudioClip[] ghoulFootsteps;


    [Header("Clown Sound Clips")]
    public AudioClip clownApperanceClip;
    public AudioClip clownJumpScareClip;

    [Header("Jack In The Box Clips")]
    public AudioClip musicBoxLoopClip;
    public AudioClip windUpCrankClip;
    public AudioClip musicBoxSongEndClip;


    [Header("Player Sound Clips")]
    public AudioClip playerSleepingClip;
    public AudioClip playerWakingUpClip;


    public void PauseAudioListener() => AudioListener.pause = true;
    public void UnpauseAudioListener() => AudioListener.pause = false;


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

    void OnEnable()
    {
        //subscribes to the events
        PauseSystem.onGamePaused += PauseAudioListener;
        PauseSystem.onGameUnpaused += UnpauseAudioListener;

    }

    void OnDisable()
    {
        //unsubscribes to the events
        PauseSystem.onGamePaused -= PauseAudioListener;
        PauseSystem.onGameUnpaused -= UnpauseAudioListener;
    }


    public void PlaySoundFXClip(AudioSource audioSourceType, AudioClip audioClip, Transform spawnTransform, bool loop, float spatialBlend, float spread)
    {
        //spawn in game object
        AudioSource audioSource = Instantiate(audioSourceType, spawnTransform.position, Quaternion.identity);

        //get the atmoky source component from the game object
        Atmoky.Source atmokySource = soundFXObject.GetComponent<Atmoky.Source>();

        //if the sound type is ui sound
        if (audioSourceType == uiSoundObject)
        {
            //make it so that the audio source doesnt stop when the game pauses
            audioSource.ignoreListenerPause = true;
        }


        //assign atmoky source values
        atmokySource.nfeDistance = 1f;
        atmokySource.nfeGain = 4f;

        //assign audio clip
        audioSource.clip = audioClip;

        //assign the audio loop
        audioSource.loop = loop;

        //assign the audio spatial blend
        audioSource.spatialBlend = spatialBlend;
        audioSource.spread = spread;

        //play sound
        audioSource.Play();


        //if loop is not enabled
        if (!loop)
        {
            //get length of sound FX clip
            float clipLength = audioSource.clip.length;

            //destroy the clip after it is done playing
            Destroy(audioSource.gameObject, clipLength);
        }
        else
        {
            return;
        }
    }


   

    public void PlaySoundFXClipAtSetVolume(AudioSource audioSourceType, AudioClip audioClip, Transform spawnTransform, bool loop, float spatialBlend, float spread, float volume)
    {
        //spawn in game object
        AudioSource audioSource = Instantiate(audioSourceType, spawnTransform.position, Quaternion.identity);

        Atmoky.Source audioSourceAtmoky = GetComponent<Atmoky.Source>();

        //if the sound type is ui sound
        if (audioSourceType == uiSoundObject)
        {
            //make it so that the audio source doesnt stop when the game pauses
            audioSource.ignoreListenerPause = true;
        }

        //assign audio clip
        audioSource.clip = audioClip;

        //assign the audio loop
        audioSource.loop = loop;


        //assign the audio spatial blend
        audioSource.spatialBlend = spatialBlend;

        audioSource.spread = spread;


        //assign the volume
        audioSource.volume = volume;

        //play sound
        audioSource.Play();

        //if loop is not enabled
        if (!loop)
        {
            //get length of sound FX clip
            float clipLength = audioSource.clip.length;

            //destroy the clip after it is done playing
            Destroy(audioSource.gameObject, clipLength);
        }
        else
        {
            return;
        }
    }


    public void PlaySoundFXClipAtSetVolumeAndRange(AudioSource audioSourceType, AudioClip audioClip, Transform spawnTransform, bool loop, float spatialBlend, float spread, float minRange, float maxRange, float volume)
    {
        //spawn in game object
        AudioSource audioSource = Instantiate(audioSourceType, spawnTransform.position, Quaternion.identity);

        Atmoky.Source audioSourceAtmoky = GetComponent<Atmoky.Source>();

        //if the sound type is ui sound
        if (audioSourceType == uiSoundObject)
        {
            //make it so that the audio source doesnt stop when the game pauses
            audioSource.ignoreListenerPause = true;
        }

        //assign audio clip
        audioSource.clip = audioClip;

        //assign the audio loop
        audioSource.loop = loop;


        //assign the audio spatial blend
        audioSource.spatialBlend = spatialBlend;

        audioSource.spread = spread;

        audioSource.minDistance = minRange;

        audioSource.maxDistance = maxRange;


        //assign the volume
        audioSource.volume = volume;

        //play sound
        audioSource.Play();

        //if loop is not enabled
        if (!loop)
        {
            //get length of sound FX clip
            float clipLength = audioSource.clip.length;

            //destroy the clip after it is done playing
            Destroy(audioSource.gameObject, clipLength);
        }
        else
        {
            return;
        }
    }



    public void PlaySoundFXClipAndAttachToGameObject(AudioSource audioSourceType, AudioClip audioClip, Transform spawnTransform, bool loop, float spatialBlend, float spread, float volume, GameObject gameObjectToAttachTo)
    {
        //spawn in game object
        AudioSource audioSource = Instantiate(audioSourceType, spawnTransform.position, Quaternion.identity);

        //Attach the audio source to the game object
        audioSource.transform.SetParent(gameObjectToAttachTo.transform);


        //get the atmoky source component from the game object
        Atmoky.Source atmokySource = soundFXObject.GetComponent<Atmoky.Source>();

        //if the sound type is ui sound
        if (audioSourceType == uiSoundObject)
        {
            //make it so that the audio source doesnt stop when the game pauses
            audioSource.ignoreListenerPause = true;
        }


        //assign atmoky source values
        atmokySource.nfeDistance = 1f;
        atmokySource.nfeGain = 4f;

        //assign audio clip
        audioSource.clip = audioClip;

        //assign the audio loop
        audioSource.loop = loop;

        //assign the audio spatial blend
        audioSource.spatialBlend = spatialBlend;

        audioSource.spread = spread;

        //assign the volume
        audioSource.volume = volume;


        //play sound
        audioSource.Play();
        

        //if loop is not enabled
        if (!loop)
        {
            //get length of sound FX clip
            float clipLength = audioSource.clip.length;

            //destroy the clip after it is done playing
            Destroy(audioSource.gameObject, clipLength);

           
        }
        else
        {
            return;
        }
    }





    public void StopSoundFXClip(AudioClip audioClip)
    {
        //find the audio source with the given audio clip
        AudioSource[] audioSourceToFindWithClip = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);

        //loop through all the audio sources
        foreach (AudioSource audioSource in audioSourceToFindWithClip)
        {
            if (audioSource.clip == audioClip)
            {
                //stop the audio source
                audioSource.Stop();

                //destroy the audio source
                Destroy(audioSource.gameObject);
            }
        }
    }

    public void StopSoundsInArray(AudioClip[] audioClips)
    {
        //find the audio source with the given audio clip
        AudioSource[] audioSourceToFindWithClip = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);

        foreach (AudioSource audioSource in audioSourceToFindWithClip) 
        {
            audioSource.Stop();

            Destroy(audioSource.gameObject);

        }
    }


    public bool IsSoundFXClipPlaying(AudioClip audioClip)
    {
        //get a list of all the audio sources
        AudioSource[] audioSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);

        //loop through all the audio sources
        foreach (AudioSource audioSource in audioSources)
        {
            //check if the audio source clip is the same as the selected audio source clip and if it is playing
            if (audioSource.clip == audioClip && audioSource.isPlaying)
            {
                //the sound is playing
                return true;
            } 
        }

        //the sound is not playing
        return false;
    }


    
}


