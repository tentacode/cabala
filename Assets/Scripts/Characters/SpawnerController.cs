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

    [SerializeField]
    private GameObject _smoke;

    [SyncVar]
    public bool ISActive = true;

    public SpawnersInformations spawnerInformations;

    private float beginTime;

    [SerializeField]
    private Transform _spawnPointWarrior;
    [SerializeField]
    private Transform _spawnPointGhost;
    [SerializeField]
    private Transform _spawnPointWizard;
    [SerializeField]
    private Transform _portal;

    [SerializeField]
    private Texture[] _texturesSpawnColorWarrior;
    [SerializeField]
    private Texture[] _texturesSpawnColorWizard;
    [SerializeField]
    private Texture[] _texturesSpawnColorGhost;
    [SerializeField]
    private Texture[] _texturesSpawnColorPortal;

    private Unit_ID _unitId;

    #endregion
    
    // Use this for initialization
    void OnEnable () {
        _unitId = GetComponent<Unit_ID>();
        Invoke("FirstSpawn", spawnerInformations.TimeBeforeFirstLaunch + 1);
    }

    bool IsInit = false;
    void Update()
    {
        _smoke.SetActive(ISActive);

        switch (spawnedCharacter)
        {
            case MinionType.Ghost:
                _smoke.transform.position = _spawnPointGhost.transform.position;
                break;
            case MinionType.Warrior:
                _smoke.transform.position = _spawnPointWarrior.transform.position;
                break;
            case MinionType.Wizard:
                _smoke.transform.position = _spawnPointWizard.transform.position;
                break;
            default:
                break;
        }

        if (IsInit || !_unitId.IsReady())
        {
            return;
        }

        IsInit = true;
        _spawnPointGhost.GetComponent<Renderer>().material.mainTexture = _texturesSpawnColorGhost[_unitId.GetPlayerIndex() - 1];
        _spawnPointWarrior.GetComponent<Renderer>().material.mainTexture = _texturesSpawnColorWarrior[_unitId.GetPlayerIndex() - 1];
        _spawnPointWizard.GetComponent<Renderer>().material.mainTexture = _texturesSpawnColorWizard[_unitId.GetPlayerIndex() - 1];
        _portal.GetComponent<Renderer>().material.mainTexture = _texturesSpawnColorPortal[_unitId.GetPlayerIndex() - 1];
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
    public void CmdisActive(bool actif)
    {
        ISActive = actif;
    }

    [Command]
    private void CmdSpawn()
    {
        if (!ISActive)
        {
            return;
        }

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
