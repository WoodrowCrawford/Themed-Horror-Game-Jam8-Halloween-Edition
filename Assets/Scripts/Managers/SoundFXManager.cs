using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;

    [SerializeField] private AudioSource soundFXObject;

    [Header("Door Clips")]
    public AudioClip doorOpenClip;
    public AudioClip doorCloseClip;



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


    private void Start()
    {

      
    }


    //search for a sound with the given name and play it


    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, bool loop, float spatialBlend)
    {
    
        //spawn in game object
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

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


   
  
}
