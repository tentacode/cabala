using UnityEngine;
using System.Collections;

public class SoundableOnStart : MonoSoundable {

    override protected void Start()
    {
        base.Start();
        playSound();
    }
}
