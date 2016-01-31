using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerAuthorityScript : NetworkBehaviour {

    private Unit_ID _unit_ID;

    void Start()
    {
        _unit_ID = GetComponent<Unit_ID>();
    }

    [Command]
    public void CmdSpawnUnit(MinionType _typeToSpawn)
    {
        Unit_ID.FindPlayer(_unit_ID.GetPlayerIndex()).GetComponent<SpawnerController>().spawnedCharacter = _typeToSpawn;
    }

    [Command]
    public void CmdOrderMinionToStop(string minionName)
    {
        GameObject.Find(minionName).GetComponent<Minions>().StopMovement();
    }
}
