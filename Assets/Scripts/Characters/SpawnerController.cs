using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
/// <summary>
/// TODO separate in two
/// </summary>
public class SpawnerController : NetworkBehaviour
{
    #region parameters
    [SyncVar]
    public MinionType spawnedCharacter;

    public SpawnersInformations spawnerInformations;

    private float beginTime;

    [SerializeField]
    private Transform _spawnPointWarrior;
    [SerializeField]
    private Transform _spawnPointGhost;
    [SerializeField]
    private Transform _spawnPointWizard;

    private Unit_ID _unitId;

    #endregion
    
    // Use this for initialization
    void OnEnable () {
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

        Transform spawnPoint;
        switch (spawnedCharacter)
        {
            case MinionType.Ghost:
                spawnPoint = _spawnPointGhost;
                break;
            case MinionType.Warrior:
                spawnPoint = _spawnPointWarrior;
                break;
            case MinionType.Wizard:
                spawnPoint = _spawnPointWizard;
                break;
            default:
                spawnPoint = _spawnPointWizard;
                break;
        }

        minion.GetComponent<Unit_SyncPosition>().CmdSetPosRot(spawnPoint.position, spawnPoint.rotation);

        minion.GetComponent<Unit_ID>().CmdSetPlayerIndex(_unitId.GetPlayerIndex());

        NetworkServer.Spawn(minion);

        Invoke("CmdSpawn", spawnerInformations.spawnSpeedCurve.Evaluate((Time.time - beginTime) / spawnerInformations.maxTime));
    }
}
