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
        CmdSpawnA();
    }

    [Command]
    public void CmdSpawnA()
    {
        GameObject o = _poolA.Pop();
        SetPosition(o);
    }

    public void ButtonSpawnB()
    {
        GameObject o = _poolB.Pop();
        SetPosition(o);
    }

    public void ButtonSpawnC()
    {
        GameObject o = _poolC.Pop();
        SetPosition(o);
    }

    void SetPosition(GameObject o)
    {
        int side = int.Parse(_inputField.text);
        
        o.GetComponent<Unit_ID>().CmdSetSideId(side);

        GameObject SpawnPoint = GameObject.Find("Player" + side);

        if (SpawnPoint == null)
        {
            Debug.LogError("Wrong Side " + side);
        }
        else
        {
            o.transform.position = SpawnPoint.transform.position;
        }
    }
}
