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
    public const int NeutralSide = 0;

    Color[] _colors =
    {
        Color.black,
        Color.white,
        Color.blue
    };

    // Side 0 is neutral
    [SerializeField]
    [SyncVar]
    private int _sideId;
    public int SideID
    {
        get
        {
            return _sideId;
        }
        set
        {
            _sideId = value;
        }
    }

    [SerializeField]
    [SyncVar]
    int life;
    int maxLife;

    public Action<GameObject, Destructible> HandleDestroyed = delegate { };
    public Action<GameObject> HandleAlive = delegate { };

    void Awake()
    {
        maxLife = life;
        SideID = SideID;
    }

    void LateUpdate()
    {
        gameObject.GetComponent<SpriteRenderer>().color = _colors[SideID];
        gameObject.layer = SideID + 10;
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
        gameObject.SetActive(true);
        life = maxLife;
        HandleAlive(gameObject);    
    }
   
}
