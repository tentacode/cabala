using UnityEngine;
using System.Collections;

public class SoundableElement : MonoBehaviour
{
    private SoundManager soundManager;
	
	void Awake () {
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void PlaySound()
    {

    }
}
