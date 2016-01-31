﻿using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class SoundableElement : MonoBehaviour
{
    [SerializeField]
    protected SoundManager soundManager;
	
	protected virtual void Start () {
        setupSoundManager();
	}

    protected void setupSoundManager()
    {
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    public virtual void LaunchSound(AudioClip sound, AudioMixerGroup mixerGroup)
    {
        if(soundManager == null)
        {
            setupSoundManager();
        }

        Debug.Log("sound is " + (sound == null) + " || mixer is  " + (mixerGroup == null) + " || soundManager is " + (soundManager == null));
        soundManager.PlaySound(sound, mixerGroup);
    }
}