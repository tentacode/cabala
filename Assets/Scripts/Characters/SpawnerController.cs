using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
/// <summary>
/// TODO separate in two
/// </summary>
public class SpawnerController : NetworkBehaviour
{
    #region parameters
    public MinionType spawnedCharacter;

    public SpawnersInformations spawnerInformations;

    private float beginTime;

    [SerializeField]
    private Transform _spawnPoint;

    private Unit_ID _unitId;

    #endregion
    
    // Use this for initialization
    void Start () {
        _unitId = GetComponent<Unit_ID>();
        Invoke("FirstSpawn", spawnerInformations.TimeBeforeFirstLaunch + 1);
    }
	
    private void FirstSpawn()
    {
        if (!isServer)
        {
            return;
        }

        beginTime = Time.time;
        CmdSpawn();
    }

    [Command]
    private void CmdSpawn()
    {
        GameObject minion = PoolManagerBase.FindPool(spawnedCharacter).Pop();
        
        minion.transform.position = _spawnPoint.position;
        minion.transform.rotation = _spawnPoint.rotation;

        minion.GetComponent<Unit_ID>().CmdSetPlayerIndex(_unitId.GetPlayerIndex());

        NetworkServer.Spawn(minion);

        Invoke("CmdSpawn", spawnerInformations.spawnSpeedCurve.Evaluate((Time.time - beginTime) / spawnerInformations.maxTime));
    }
}
