using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Networking;

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

    public Minions opponent { get; private set; }

    private MinionState _state;
    private NavMeshAgent navAgent;
    private Transform goal;
    private bool isInit = false;


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
        if (!isServer)
        {
            return;
        }

        DEBUGState = state;

        if (_unit_ID.IsReady())
        {
            initalize();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isServer)
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
        if (!isServer)
        {
            return;
        }

        if (isInit)
        {
            return;
        }
        isInit = true;

        state = MinionState.moving;

        

        int numberOfPlayer = GameObject.Find("GameSharedData").GetComponent<GameSharedData>().NumberOfPlayer;

        Debug.Log((PlayerIndex % numberOfPlayer) + 1);
        GameObject player = Unit_ID.FindPlayer((PlayerIndex % numberOfPlayer) + 1);
        SetGoal(player.transform);
    }

    void Update()
    {
        setMaterial();
    }

    private void setMaterial()
    {

        Debug.Log("SetMaterial " + (PlayerIndex-1) + " " + minionsInformations.teamMaterials[PlayerIndex-1]);
        GetComponent<Renderer>().material = minionsInformations.teamMaterials[PlayerIndex-1];
    }

    public void SetGoal(Transform goalTransform)
    {
        if (!isServer)
        {
            return;
        }

        Debug.Log(name + " goto " + goalTransform.name); 

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
        if (!isServer)
        {
            return;
        }

        if (state == MinionState.fighting)
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
        if (!isServer)
        {
            return;
        }

        if (_destructible.GetLife() <= 0)
        {
            return; // do nothing, will go on late state and die as it should
        }

        opponent = null;
        state = previousState;

        CancelInvoke("Attack");

        setupMoving();
        
    }

    void setupMoving()
    {
        if (!isServer)
        {
            return;
        }

        if (state == MinionState.moving)
        {
            navAgent.destination = goal.position;
            navAgent.Resume();
        }
    }

    public void setupFight()
    {
        if (!isServer)
        {
            return;
        }

        state = MinionState.fighting;
        navAgent.Stop();
    }

    public void Attack()
    {
        if (!isServer)
        {
            
            return;
        }

        if (opponent == null)
        {
            state = MinionState.moving;
            setupMoving();
            return;
        }

        Destructible opponentDestructible = opponent.GetComponent<Destructible>();
        opponentDestructible.CmdTakeDamage(computeDamages());
        _destructible.CmdTakeDamage(opponent.computeDamages());

        Invoke("Attack", minionsInformations.attackSpeed);
    }

    private int computeDamages()
    {
        if (!isServer || opponent == null)
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
        if (!isServer)
        {
            return;
        }

        if (opponent != null)
        {
            opponent.finishFight();
        }

        state = MinionState.dead;

        //      Destroy(gameObject);
   }
    private void OnAlive(GameObject whoAlive)
    {
        if (!isServer)
        {
            return;
        }

        isInit = false;
        initalize();
    }

    #endregion
}
