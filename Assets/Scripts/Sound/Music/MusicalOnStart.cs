using UnityEngine;
using System.Collections;

public class MusicalOnStart : MusicalMono
{
    public float secondsBeforePlay = 0f;
    public bool stopMusicRightAway = false;

    override protected void Start()
    {
        base.Start();

        if(stopMusicRightAway)
        {
            musicManager.Stop();
        }

        Invoke("playSound", secondsBeforePlay);
    }
}
