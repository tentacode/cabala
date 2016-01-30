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
    [SerializeField]
    [SyncVar]
    int life;
    public int maxLife;

    public Action<GameObject, Destructible> HandleDestroyed = delegate { };
    public Action<GameObject> HandleAlive = delegate { };

    private Unit_ID unitId;

    public int GetLife()
    {
        return life;
    }

    void Awake()
    {
        life = maxLife;
        unitId = GetComponent<Unit_ID>();
    }

    public void TakeDamage(int damage, Destructible attaking)
    {
        life -= damage;
        if (life <= 0)
        {
            GoDead(attaking);
        }
    }

    public void GoDead(Destructible attaking)
    {
        GoDeadNoBroadcart();
        HandleDestroyed(gameObject, attaking);
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
