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

    public bool overrideMaterial = false;
    public bool overrideHealthBarMaterial = true;

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
    private Destructible _destructible;
    private Unit_ID _unit_ID;
    private SoundableMinion minionSoundControler;

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

        minionSoundControler = GetComponent<SoundableMinion>();
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

        minionSoundControler.PlaySound(MinionAction.spawn);

        GameObject player = GameSharedData.GetPlayerNumberNext(_unit_ID, 1);
        SetGoal(player.transform);

        setMaterial();
        setHealthBarMaterial();
    }

    private void setMaterial()
    {
        if (overrideMaterial)
        {
            if (minionsInformations.getMinionMaterials(minionType)[PlayerIndex - 1] != null)
            {
                GetComponent<Renderer>().material = minionsInformations.getMinionMaterials(minionType)[PlayerIndex - 1];
            }
        }
    }

    private void setHealthBarMaterial()
    {
        if(overrideHealthBarMaterial)
        {
            GameObject healthBar = transform.FindChild("LifeBar").gameObject;
            if(healthBar != null)
            {
                healthBar.GetComponent<Renderer>().material = minionsInformations.healBarTeamMaterials[PlayerIndex - 1];
            }
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

        minionSoundControler.PlaySound(MinionAction.attack);
        opponent.minionSoundControler.PlaySound(MinionAction.attack);

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

        minionSoundControler.PlaySound(MinionAction.die);
    }

    private void OnAlive(GameObject whoAlive)
    {
        if (!CanRun)
        {
            return;
        }

        isInit = false;
        //initalize();
    }

    #endregion
}
