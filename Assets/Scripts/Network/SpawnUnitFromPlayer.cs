using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SpawnUnitFromPlayer : NetworkBehaviour {

    private PlayerNetwork _playerNetwork;

    void Start()
    {
        _playerNetwork = GetComponent<PlayerNetwork>();
    }


    [Command]
    public void CmdSpawn(string poolName)
    {
        PoolManagerBase pool = GameObject.Find(poolName).GetComponent<PoolManagerBase>();

        Debug.Log(poolName + " " + pool);
        GameObject o = pool.Pop();

        int side = _playerNetwork.playerIndex;

        o.GetComponent<Unit_ID>().CmdSetPlayerNumber(side);

        o.transform.position = transform.position;
        
    }
   
}
