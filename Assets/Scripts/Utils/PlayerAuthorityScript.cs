using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerAuthorityScript : NetworkBehaviour {

    private Unit_ID _unit_ID;

    [SyncVar]
    public int cultisteLife = 5;

    [SerializeField]
    private InvocationCircleControler _invoc;

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
        GameObject.Find(minionName).GetComponent<Minions>().MovementStop();
    }

    [Command]
    public void CmdOrderMinionToMoveTo(string minionName, string targetName)
    {
        GameObject target = GameObject.Find(targetName);

        GameObject.Find(minionName).GetComponent<Minions>().MovementGoTo(target.transform.position);
    }

    [Command]
    public void CmdSetLife(int life)
    {
        this.cultisteLife = life;
    }

    [Command]
    public void CmdDestroyCultiste(string name)
    {
        cultisteLife--;

        GameObject minion = GameObject.Find(name);
        if (minion != null)
        {
            minion.GetComponent<Destructible>().CmdTakeDamage(1000);
        }

        _invoc.CultistDeath(name);
    }
    [SyncVar]
    public bool IsGameOver = false;

    [SyncVar]
    bool rematch = false;

    [Command]
    public void CmdGameOver()
    {
        IsGameOver = true;
    }

    bool GameOverDOne = false;
    void Update()
    {
        if (rematch)
        {
            GameOverDOne = false;
            IsGameOver = false;
            InvocationCircleControler.PlayerDead = 0;

            foreach (var p in GameSharedData.GetAllPlayers)
            {
                p.GetComponent<PlayerAuthorityScript>().CmdSetLife(5);
            }
        }

        if (IsGameOver == false)
        {
            GameOverDOne = false;
        }

        if (IsGameOver && !GameOverDOne)
        {
            GameOverDOne = true;
            GameObject.Find("GameOverEffects").GetComponent<UIDeath>().Activate(true);
        }
    }

    [Command]
    public void CmdRematch()
    {
        rematch = true;


    }
}
