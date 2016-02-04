using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class MusicalElement : MonoBehaviour
{
    protected MusicManager musicManager;

    [SerializeField]
    protected bool loop;

    protected virtual void Start()
    {
        setupMusicManager();
    }

    protected void setupMusicManager()
    {
        musicManager = GameObject.Find("MusicManager").GetComponent<MusicManager>();
    }

    public virtual void LaunchMusic(AudioClip Music, AudioMixerGroup mixerGroup)
    {
        if (musicManager == null)
        {
            setupMusicManager();
        }

        musicManager.PlayMusic(Music, mixerGroup, loop);
    }

    public void StopMusicManager()
    {
        musicManager.Stop();
    }
}
