using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using UnityEngine.Networking;

public class SoundManager : NetworkBehaviour
{
    private AudioSource audioSource;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PlaySound(AudioClip sound, AudioMixerGroup mixerGroup)
    {
        audioSource.clip = sound;
        audioSource.outputAudioMixerGroup = mixerGroup;
        audioSource.Play();
    }
}
