using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;



public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;

    [SerializeField] private AudioSource soundFXObject;

    [Header("Button Click Clips")]
    public AudioClip buttonHoverClip;
    public AudioClip buttonClickClip;
    public AudioClip backButtonClickClip;


    [Header("Door Clips")]
    public AudioClip doorOpenClip;
    public AudioClip doorCloseClip;

    [Header("Wall Clock Clips")]
    public AudioClip tickingClockClip;

    [Header("Flashlight Clips")]
    public AudioClip flashlightClickOnClip;
    public AudioClip flashlightClickOffClip;

    [Header("Jack In The Box Clips")]
    public AudioClip musicBoxLoopClip;
    public AudioClip windUpCrankClip;
    public AudioClip musicBoxSongEndClip;



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


  

    //search for a sound with the given name and play it


    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, bool loop, float spatialBlend)
    {
        //spawn in game object
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        //get the atmoky source component from the game object
        AtmokySource atmokySource = soundFXObject.GetComponent<AtmokySource>();

        //assign atmoky source values
        atmokySource.nfeDistance = 1f;
        atmokySource.nfeGain = 4f;

        //assign audio clip
        audioSource.clip = audioClip;

        //assign the audio loop
        audioSource.loop = loop;

        //assign the audio spatial blend
        audioSource.spatialBlend = spatialBlend;

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

    
    public void PlaySoundFXClipAtSetVolume(AudioClip audioClip, Transform spawnTransform, bool loop, float spatialBlend, float volume)
    {
        //spawn in game object
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
    
        AtmokySource audioSourceAtmoky = GetComponent<AtmokySource>();
        

   

        //assign audio clip
        audioSource.clip = audioClip;

        //assign the audio loop
        audioSource.loop = loop;


        //assign the audio spatial blend
        audioSource.spatialBlend = spatialBlend;


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
        AudioSource audioSourceToFindWithClip = FindObjectOfType<AudioSource>();

        if(audioSourceToFindWithClip.GetComponentInChildren<AudioSource>().clip = audioClip)
        {
            Destroy(audioSourceToFindWithClip.gameObject);
        }
    }

   
  
}
