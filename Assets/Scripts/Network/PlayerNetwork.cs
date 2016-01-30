using UnityEngine;
using System.Collections;

public class PlayerNetwork : MonoBehaviour
{
    private int playerIndex;
    
	void Awake () 
    {
	   DontDestroyOnLoad(gameObject);
	}
    
    public void SetPlayerIndex(int idx)
    {
        playerIndex = idx;
    }
    
    public void InitPosition()
    {
        var ground = GameObject.Find("Ground" + playerIndex);
        transform.position = ground.transform.position;
    }
}
