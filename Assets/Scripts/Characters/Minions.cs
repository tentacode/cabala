﻿using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public enum MinionState
{
    stop,
    moving,
    fighting,
    dead
}

[Serializable]
public enum MinionType
{
    ghost,
    warrior,
    wizard
}

public class Minions : MonoBehaviour
{
    #region parameters
    public MinionsInformations minionsInformations;
    public int playerNumber;
    public MinionType minionType;

    [SerializeField]
    public Dictionary<MinionType, MinionType> StongAgainst;
    
    public MinionState DEBUGState;
    
    public MinionState state
    {
        get
        {
            return _state;
        }
        set
        {
            if (value != state)
            {
                previousState = _state;
                _state = value;
            }
        }
    }
    public MinionState previousState { get; protected set; }

    public int ownerIndex { get; private set; }
    
    public Minions opponent { get; private set; }

    private MinionState _state;
    private NavMeshAgent navAgent;
    private Transform goal;
    private bool isInit = false;
    private int currentLife = 10;
    private GameObject lifeBar;
    
    #endregion

    #region engine methods

    /*
    void Awake()
    {

    } */

    void Start () {
        initalize();
    }

    void LateUpdate()
    {
        CheckDeath();
        DEBUGState = state;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Minion")
        {
            Minions opponent = other.GetComponent<Minions>();
            if (opponent.ownerIndex != ownerIndex && opponent.state != MinionState.fighting)
            {
                LaunchFight(other.GetComponent<Minions>());
            }
        }
    }

    #endregion

    #region methods

    protected void initalize()
    {
        if(isInit)
        {
            return;
        }
        
        navAgent = GetComponent<NavMeshAgent>();
        playerNumber = GameObject.Find("GameSharedData").GetComponent<GameSharedData>().PlayerNumber;
        lifeBar = transform.FindChild("LifeBar").gameObject;
        currentLife = minionsInformations.baseLifePoints;

        state = MinionState.moving;
    }

    private void setMaterial()
    {
        GetComponent<Renderer>().material = minionsInformations.teamMaterials[ownerIndex - 1];
    }
    
    public void SetOwnerNumber(int owner)
    {
        if(!isInit)
        {
            initalize();
        }

        ownerIndex = owner;

        setMaterial();

        SetGoal(GameObject.Find("Player" + ((ownerIndex%playerNumber) + 1)).transform);
    }

    public void SetGoal(Transform goalTransform)
    {
        if (!isInit)
        {
            initalize();
        }

        goal = goalTransform;
        if (goal != null)
        {
            navAgent.destination = goal.position;
        }
        else
        {
            navAgent.Stop();
        }
    }

    public void LaunchFight(Minions otherFighter)
    {
        if(state == MinionState.fighting)
        {
            return;
        }

        opponent = otherFighter;
        opponent.opponent = this;

        setupFight();
        opponent.setupFight();
        
        Invoke("Attack", minionsInformations.firstAttackSpeed);
    }

    public void finishFight()
    {
        if (currentLife <= 0)
        {
            return; // do nothing, will go on late state and die as it should
        }

        opponent = null;
        state = previousState;

        CancelInvoke("Attack");

        if (state == MinionState.moving)
        {
            navAgent.destination = goal.position;
            navAgent.Resume();
        }
    }

    public void setupFight()
    {
        state = MinionState.fighting;
        navAgent.Stop();
    }

    public void Attack()
    {
        opponent.TakeDamage(computeDamages());
        TakeDamage(opponent.computeDamages());
        
        Invoke("Attack", minionsInformations.attackSpeed);
    }

    public int computeDamages()
    {
        if (minionsInformations.minionStrengths.amIStrongAgainst(minionType, opponent.minionType))
        {
            return(int) ((float)minionsInformations.damages * minionsInformations.damageMultiplicator);
        }

        return minionsInformations.damages;
    }

    public void TakeDamage(int damages)
    {
        currentLife -= damages;

        if(lifeBar == null)
        {
            return;
        }

        lifeBar.transform.localScale = new Vector3(lifeBar.transform.localScale.x,
                                                   lifeBar.transform.localScale.y,
                                                   Mathf.Max((float)currentLife / (float)minionsInformations.baseLifePoints, 0f));
    }

    public void CheckDeath()
    {
        if(currentLife <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        opponent.finishFight();
        state = MinionState.dead;

        Destroy(gameObject);
    }

    #endregion
}