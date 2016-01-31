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
        var spawn = GameObject.Find("SpawnPlayer" + _unitID.GetPlayerIndex());
        transform.position = spawn.transform.position;
        transform.rotation = spawn.transform.rotation;
    }
    
    void InitCamera()
    {
        if (!isLocalPlayer) {
            return;
        }
     
        GameObject cameraTarget = GameObject.Find("CameraTarget");
        var playerIndex = _unitID.GetPlayerIndex();

        switch (playerIndex) {
            case 2:
                cameraTarget.transform.Rotate(new Vector3(0, -90, 0));
                break;
            case 3:
                cameraTarget.transform.Rotate(new Vector3(0, 180, 0));
                break;
            case 4:
                cameraTarget.transform.Rotate(new Vector3(0, 90, 0));
                break;
        }

    }
}
