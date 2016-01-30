using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class UISpawn : NetworkBehaviour {

    [SerializeField]
    private PoolManagerBase _poolA;
    [SerializeField]
    private PoolManagerBase _poolB;
    [SerializeField]
    private PoolManagerBase _poolC;

    [SerializeField]
    private InputField _inputField;

    public void ButtonSpawnA()
    {
        int side = int.Parse(_inputField.text);
        SpawnUnitFromPlayer Spawner = GameObject.Find("Player" + side).GetComponent<SpawnUnitFromPlayer>();

        Spawner.CmdSpawn(_poolA.name);
    }

    public void ButtonSpawnB()
    {
        int side = int.Parse(_inputField.text);
        SpawnUnitFromPlayer Spawner = GameObject.Find("Player" + side).GetComponent<SpawnUnitFromPlayer>();

        Spawner.CmdSpawn(_poolB.name);
    }

    public void ButtonSpawnC()
    {
        int side = int.Parse(_inputField.text);
        SpawnUnitFromPlayer Spawner = GameObject.Find("Player" + side).GetComponent<SpawnUnitFromPlayer>();

        Spawner.CmdSpawn(_poolC.name);
    }
   
}
