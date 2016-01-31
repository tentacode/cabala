using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class SoundableElement : MonoBehaviour
{
    protected SoundManager soundManager;
	
	protected virtual void Start () {
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
	}

    public virtual void LaunchSound(AudioClip sound, AudioMixerGroup mixerGroup)
    {
        soundManager.PlaySound(sound, mixerGroup);
    }
}
