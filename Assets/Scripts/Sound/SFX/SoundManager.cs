using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using UnityEngine.Networking;

public class SoundManager : MonoBehaviour
{
    private AudioSource audioSource;

	// Use this for initialization
	void Awake () {
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
	}

    public void PlaySound(AudioClip sound, AudioMixerGroup mixerGroup)
    {
        audioSource.clip = sound;
        audioSource.outputAudioMixerGroup = mixerGroup;
        audioSource.Play();
    }
}
