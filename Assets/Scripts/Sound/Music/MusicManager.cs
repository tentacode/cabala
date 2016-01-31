using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    private AudioSource audioSource;

    private AudioClip lastLoopingMusic;
    private AudioMixerGroup lastLoopingMusicMixerGroup;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayMusic(AudioClip music, AudioMixerGroup mixerGroup, bool looping)
    {
        audioSource.clip = music;
        audioSource.outputAudioMixerGroup = mixerGroup;
        audioSource.loop = looping;
        audioSource.Play();

        if(!looping)
        {
            Invoke("PlayLastLoopingMusic", music.length);
        }
        else
        {
            lastLoopingMusic = music;
            lastLoopingMusicMixerGroup = mixerGroup;
        }
    }

    private void PlayLastLoopingMusic()
    {
        if(lastLoopingMusic == null)
        {
            return;
        }

        PlayMusic(lastLoopingMusic, lastLoopingMusicMixerGroup, true);
    }

    public void Stop()
    {
        audioSource.Stop();
    }
}
