using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Invoke("prout", 1f);
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void prout()
    {
        GetComponent<MonoSoundable>().playSound();
        Debug.Log("caca");
        Invoke("prout", 1.0f);
    }
}
