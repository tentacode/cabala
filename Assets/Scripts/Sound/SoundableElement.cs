using UnityEngine;
using System.Collections;

public abstract class SoundableElement : MonoBehaviour
{
    private SoundManager soundManager;
	
	void Awake () {
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    protected virtual void PlaySound()
    {

    }
}
