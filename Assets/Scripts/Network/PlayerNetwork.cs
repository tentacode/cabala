using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerNetwork : NetworkBehaviour
{
    private Unit_ID _unitID;

    [SyncVar]
    public int connectionId;
    
    private bool initialized = false;
    
	void Awake () 
    {
	   DontDestroyOnLoad(gameObject);
       _unitID = GetComponent<Unit_ID>();
	}
    
    void Update ()
    {
        if (_unitID.GetPlayerIndex() != 0 && !initialized)
        {
            InitGame();
        }
    }
    
    public void InitGame()
    {
        InitPosition();
        InitCamera();
        initialized = true;
    }
    
    void InitPosition()
    {
        var ground = GameObject.Find("SpawnPlayer" + _unitID.GetPlayerIndex());
        transform.position = ground.transform.position;
    }
    
    void InitCamera()
    {
        if (!isLocalPlayer) {
            return;
        }
     
        Debug.Log("InitCamera");   
        GameObject cameraTarget = GameObject.Find("CameraTarget");
        cameraTarget.transform.position = transform.position;
        cameraTarget.transform.rotation = transform.rotation;
    }
}
