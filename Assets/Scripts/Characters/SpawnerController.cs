using UnityEngine;
using System.Collections;

public class SpawnerController : MonoBehaviour
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
        beginTime = Time.time;
        Spawn();
    }

    private void Spawn()
    {
        GameObject minion = PoolManagerBase.FindPool(spawnedCharacter).Pop();
        minion.transform.position = _spawnPoint.position;
        minion.transform.rotation = _spawnPoint.rotation;

        minion.GetComponent<Unit_ID>().CmdSetPlayerNumber(_unitId.GetPlayerNumber());
        
        Invoke("Spawn", spawnerInformations.spawnSpeedCurve.Evaluate((Time.time - beginTime) / spawnerInformations.maxTime));
    }
}
