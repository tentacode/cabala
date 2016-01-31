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

        Debug.Log("MaxLife change " + maxLife);
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
        if (life <= 0)
        {
            GoDead();
        }

        HandleTakeDamage(gameObject, damage);
    }

    private void GoDead()
    {
        GoDeadNoBroadcart();
        HandleDestroyed(gameObject);
    }

    public void GoDeadNoBroadcart()
    {
        gameObject.SetActive(false);
    }

    public void GoAlive()
    {
        //yield return null;
      //  yield return new WaitForSeconds(1);

        /*while (!unitId.IsReady())
        {
            yield return null;
        }*/

        

        gameObject.SetActive(true);
        life = maxLife;
        HandleAlive(gameObject);  
    }
}
