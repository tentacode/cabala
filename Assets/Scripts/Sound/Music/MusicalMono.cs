using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class MusicalMono : MusicalElement
{
    [SerializeField]
    protected AudioClip music;
    [SerializeField]
    protected AudioMixerGroup mixerGroup;

    public void playSound()
    {
        LaunchMusic(music, mixerGroup);
    }
}
