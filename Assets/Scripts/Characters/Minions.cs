using UnityEngine;
using System.Collections;
using System;

public enum MinionState
{
    stop,
    moving,
    fighting,
    dead
}

public class Minions : MonoBehaviour
{
    #region parameters
    public MinionsInformations minionsInformations;
    public int playerNumber;
    public bool DEBUGFightInitator = false;

    public MinionState state { get; protected set; }
    public int ownerIndex { get; private set; }
    
    public Minions opponent { get; private set; }

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
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Minion")
        {
            Minions opponent = other.GetComponent<Minions>();
            if (opponent.ownerIndex != ownerIndex && opponent.opponent != this)
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

        state = MinionState.stop;
    }

    private void setMaterial()
    {
        Debug.Log(ownerIndex - 1);
        Debug.Log(minionsInformations.teamMaterials.Length);

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

        Debug.Log("LaunchFight");

        DEBUGFightInitator = true;

        opponent = otherFighter;
        opponent.opponent = this;

        setupFight();
        opponent.setupFight();
        
        Invoke("Attack", minionsInformations.firstAttackSpeed);
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
        return minionsInformations.damages;
    }

    public void TakeDamage(int damages)
    {
        currentLife -= damages;

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
        state = MinionState.dead;

        Destroy(gameObject);
    }

    #endregion
}
