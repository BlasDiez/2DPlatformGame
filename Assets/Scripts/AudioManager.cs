using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    private AudioSource _audioSource;
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    public void PlaySound(AudioClip audio)
    {
        _audioSource.PlayOneShot(audio);
    }
}