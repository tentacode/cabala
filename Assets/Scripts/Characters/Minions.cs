using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Networking;

[Serializable]
public enum MinionAction
{
    attack,
    spawn,
    die
}

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
    Ghost,
    Warrior,
    Wizard
}

public class Minions : NetworkBehaviour
{
    #region parameters
    public MinionsInformations minionsInformations;
    public MinionType minionType;

    [SerializeField]
    public Dictionary<MinionType, MinionType> StongAgainst;

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

    public Minions opponent { get; private set; }

    private MinionState _state;
    private NavMeshAgent navAgent;
    private Transform goal;
    private bool isInit = false;

    public bool overrideMaterial = false;


    private Destructible _destructible;
    private Unit_ID _unit_ID;

    public int PlayerIndex
    {
        
        get
        {
            return _unit_ID.GetPlayerIndex();
        }
    }

    #endregion

    #region engine methods

    public bool CanRun
    {
        get
        {
            return isServer && gameObject.activeSelf;
        }
    }

    public override void OnStartServer()
    {
        _destructible = GetComponent<Destructible>();
        navAgent = GetComponent<NavMeshAgent>();
        _unit_ID = GetComponent<Unit_ID>();
        _destructible.CmdSetMaxLife(minionsInformations.baseLifePoints);

        _destructible.HandleDestroyed += OnDie;
        _destructible.HandleAlive += OnAlive;
    }

    void Awake()
    {
        _unit_ID = GetComponent<Unit_ID>();
    }



    void LateUpdate()
    {
        if (!CanRun)
        {
            return;
        }

        if (_unit_ID.IsReady())
        {
            initalize();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!CanRun)
        {
            return;
        }

        if (other.tag == "Minion")
        {
            Minions opponent = other.GetComponent<Minions>();
            if (opponent.PlayerIndex != PlayerIndex && opponent.state != MinionState.fighting)
            {
                LaunchFight(opponent);
            }
        }
    }

    #endregion

    #region methods


    protected void initalize()
    {
        if (!CanRun)
        {
            return;
        }

        if (isInit)
        {
            return;
        }
        isInit = true;

        state = MinionState.moving;



        GameObject player = GameSharedData.GetPlayerNumberNext(_unit_ID, 1);
        SetGoal(player.transform);
    }

    void Update()
    {
        setMaterial();
    }

    private void setMaterial()
    {
        if (overrideMaterial)
        {
            GetComponent<Renderer>().material = minionsInformations.healBarTeamMaterials[PlayerIndex - 1];
        }
    }

    public void SetGoal(Transform goalTransform)
    {
        if (!CanRun)
        {
            return;
        }

        Debug.Log(name + " goto " + goalTransform.name); 

        goal = goalTransform;
        if (goal != null)
        {
            MovementGoTo(goal.position);
        }
        else
        {
            MovementStop();
        }
    }

    public void LaunchFight(Minions otherFighter)
    {
        if (!CanRun)
        {
            return;
        }

        if (state == MinionState.fighting)
        {
            return;
        }

        opponent = otherFighter;
        opponent.opponent = this;

        transform.LookAt(opponent.transform);
        opponent.transform.LookAt(transform);

        setupFight();
        opponent.setupFight();

        Invoke("Attack", minionsInformations.firstAttackSpeed);
    }

    public void finishFight()
    {
        if (!CanRun)
        {
            return;
        }

        opponent = null;
        state = previousState;

        CancelInvoke("Attack");

        setupMoving();
        
    }

    void setupMoving()
    {
        if (!CanRun)
        {
            return;
        }

        
        MovementGoTo(goal.position);
    }


    public void setupFight()
    {
        if (!CanRun)
        {
            return;
        }

        state = MinionState.fighting;
        MovementStop();
    }

    public void MovementGoTo(Vector3 objectif)
    {
        if (state != MinionState.moving)
        {
            return;
        }

         navAgent.destination = objectif;
         navAgent.Resume();
    }

    public void MovementStop()
    {
        navAgent.Stop();
    }

    public void Attack()
    {
        if (!CanRun)
        {
            
            return;
        }

        Destructible opponentDestructible = opponent.GetComponent<Destructible>();
        opponentDestructible.CmdTakeDamage(computeDamages());

        _destructible.CmdTakeDamage(opponent.computeDamages());

        Invoke("Attack", minionsInformations.attackSpeed);
    }

    private int computeDamages()
    {
        if (!CanRun || opponent == null)
        {
            return 0;
        }

        if (minionsInformations.minionStrengths.amIStrongAgainst(minionType, opponent.minionType))
        {
            return (int)((float)minionsInformations.damages * minionsInformations.damageMultiplicator);
        }

        return minionsInformations.damages;
    }

    private void OnDie(GameObject whoDied)
    {
        if (!CanRun)
        {
            return;
        }

        if (opponent != null)
        {
            opponent.finishFight();
        }

        state = MinionState.dead;
   }
    private void OnAlive(GameObject whoAlive)
    {
        if (!CanRun)
        {
            return;
        }

        isInit = false;
        initalize();
    }

    #endregion
}
