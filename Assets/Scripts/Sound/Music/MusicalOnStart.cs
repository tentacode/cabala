using UnityEngine;
using System.Collections;

public class MusicalOnStart : MusicalMono
{
    override protected void Start()
    {
        base.Start();
        playSound();
    }
}
