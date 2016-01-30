using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SpawnUnitFromPlayer : NetworkBehaviour {

    private PlayerNetwork _playerNetwork;

    private Unit_ID _unitID;

    void Start()
    {
        _playerNetwork = GetComponent<PlayerNetwork>();
        _unitID = GetComponent<Unit_ID>();
    }


    [Command]
    public void CmdSpawn(string poolName)
    {
        PoolManagerBase pool = GameObject.Find(poolName).GetComponent<PoolManagerBase>();

        Debug.Log(poolName + " " + pool);
        GameObject o = pool.Pop();

        int side = _unitID.GetPlayerNumber();

        o.GetComponent<Unit_ID>().CmdSetPlayerNumber(side);

        o.transform.position = transform.position;

        NetworkServer.Spawn(o);
        
    }
   
}
