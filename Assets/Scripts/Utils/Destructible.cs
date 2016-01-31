#define DIE_ACTIVATION
//#define DIE_MOVE

//#define FADEINOUT

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Networking;

public class Destructible : NetworkBehaviour
{
    [SyncVar]
    int life;
    [SyncVar]
    int maxLife;

    public Action<GameObject> HandleDestroyed = delegate { };
    public Action<GameObject> HandleAlive = delegate { };
    public Action<GameObject, int> HandleTakeDamage = delegate { };

    private Unit_ID unitId;

    public int GetLife()
    {
        return life;
    }

    public int GetMaxLife()
    {
        return maxLife;
    }

    [Command]
    public void CmdSetMaxLife(int v)
    {
        maxLife = v;

        life = maxLife;
    }

    void Awake()
    {
        unitId = GetComponent<Unit_ID>();

        if (!isServer)
        {
            return;
        }
    }

    [Command]
    public void CmdTakeDamage(int damage)
    {
        life -= damage;
        HandleTakeDamage(gameObject, damage);
    }

    void LateUpdate()
    {
        if (!isServer)
        {
            return;
        }

        if (life <= 0)
        {
            GoDead();
        }
    }

    private void GoDead()
    {
        HandleDestroyed(gameObject);

        GoDeadNoBroadcart();
    }

    public void GoDeadNoBroadcart()
    {
        NetworkServer.UnSpawn(gameObject);
        Destroy(gameObject);
    }

    public void GoAlive()
    {   
        gameObject.SetActive(true);
        life = maxLife;
        HandleAlive(gameObject);  
    }
}
