using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ServerMusic : NetworkBehaviour
{
	void Start ()
    {
        if (!isServer) {
            MusicManager musicManager = GameObject.Find("MusicManager").GetComponent<MusicManager>();
            musicManager.Stop();
            MusicalOnStart music = GetComponent<MusicalOnStart>();
            music.enabled = false;
        }
	}
}
