using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class MonoSoundable : SoundableElement {

    [SerializeField]
    protected AudioClip sound;
    [SerializeField]
    protected AudioMixerGroup mixerGroup;
	
    public void playSound()
    {
        LaunchSound(sound, mixerGroup);
    }
}
