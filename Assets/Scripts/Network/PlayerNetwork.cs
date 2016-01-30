using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerNetwork : NetworkBehaviour
{
    [SyncVar]
    public int playerIndex = 0;
    
    [SyncVar]
    public int connectionId;
    
    private bool initialized = false;
    
	void Awake () 
    {
	   DontDestroyOnLoad(gameObject);
	}
    
    void Update ()
    {
        if (playerIndex != 0 && !initialized) {
            InitGame();
        }
    }
    
    public void InitGame()
    {
        gameObject.name = "Player" + playerIndex;
        
        InitPosition();
        InitCamera();
        initialized = true;
    }
    
    void InitPosition()
    {
        var ground = GameObject.Find("Ground" + playerIndex);
        transform.position = ground.transform.position;
    }
    
    void InitCamera()
    {
        if (!isLocalPlayer) {
            return;
        }
        
        Debug.Log("Camera Init");
        
        GameObject cameraTarget = GameObject.Find("CameraTarget");
        cameraTarget.transform.position = transform.position;
        cameraTarget.transform.rotation = transform.rotation;
    }
}
