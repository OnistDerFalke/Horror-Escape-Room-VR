using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioClip firstRoomSound;
    [SerializeField] private AudioClip storageSound;
    [SerializeField] private AudioClip secondRoomSound;
    
    [SerializeField] private AudioClip jumpscare1Sound;
    [SerializeField] private AudioClip pickItemSound;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource eventSource;

    private bool[] wasGlobalSoundPlayed = new bool[(int)GameManager.GlobalSoundType.SoundsNumber];
    private void Start()
    {
        PlayFirstRoomDefaultSound();
    }

    private void Update()
    {
        switch(GameManager.SoundType)
        {
            case GameManager.GlobalSoundType.FirstRoom:
                if(!wasGlobalSoundPlayed[(int)GameManager.GlobalSoundType.FirstRoom])
                    PlayFirstRoomDefaultSound();
                wasGlobalSoundPlayed[(int)GameManager.GlobalSoundType.FirstRoom] = true;
                break;
            case GameManager.GlobalSoundType.Storage:
                if(!wasGlobalSoundPlayed[(int)GameManager.GlobalSoundType.Storage])
                    PlayStorageDefaultSong();
                wasGlobalSoundPlayed[(int)GameManager.GlobalSoundType.Storage] = true;
                break;
            case GameManager.GlobalSoundType.SecondRoom:
                if(!wasGlobalSoundPlayed[(int)GameManager.GlobalSoundType.SecondRoom])
                    PlaySecondRoomDefaultSound();
                wasGlobalSoundPlayed[(int)GameManager.GlobalSoundType.SecondRoom] = true;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void PlayJumpscare1Sound()
    {
        eventSource.clip = jumpscare1Sound;
        eventSource.loop = false;
        eventSource.spatialBlend = 1.0f;
        eventSource.volume = 2.0f;
        eventSource.Play();
    }
    
    public void PlayPickItemSound()
    {
        eventSource.clip = pickItemSound;
        eventSource.loop = false;
        eventSource.spatialBlend = 1.0f;
        eventSource.volume = 1.0f;
        eventSource.Play();
    }
    
    private void PlayFirstRoomDefaultSound()
    {
        audioSource.clip = firstRoomSound;
        audioSource.loop = true;
        audioSource.spatialBlend = 1.0f;
        audioSource.volume = 0.5f;
        audioSource.Play();
    }

    private void PlayStorageDefaultSong()
    {
        audioSource.clip = storageSound;
        audioSource.loop = true;
        audioSource.spatialBlend = 1.0f;
        audioSource.volume = 0.2f;
        audioSource.Play();
    }

    private void PlaySecondRoomDefaultSound()
    {
        audioSource.clip = secondRoomSound;
        audioSource.loop = true;
        audioSource.spatialBlend = 1.0f;
        audioSource.volume = 0.2f;
        audioSource.Play();
    }
}
