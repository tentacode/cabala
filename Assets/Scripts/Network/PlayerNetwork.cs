using UnityEngine;
using System.Collections;

public class PlayerNetwork : MonoBehaviour
{
    public int playerId;
    
	void Awake () 
    {
	   DontDestroyOnLoad(gameObject);
	}
}
