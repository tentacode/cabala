using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerNetwork : NetworkBehaviour
{
    [SyncVar]
    public int playerIndex;
    
	void Awake () 
    {
	   DontDestroyOnLoad(gameObject);
	}
    
    public void InitGame()
    {
        InitPosition();
        InitCamera();
    }
    
    void InitPosition()
    {
        var ground = GameObject.Find("Ground" + playerIndex);
        transform.position = ground.transform.position;
    }
    
    void InitCamera()
    {
        Debug.Log("Init camera");
        GameObject cameraTarget = GameObject.Find("CameraTarget");
        cameraTarget.transform.position = transform.position;
        cameraTarget.transform.rotation = transform.rotation;
    }
    
    void Update()
    {
        gameObject.name = "Player"+playerIndex;
    }
}
