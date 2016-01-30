using UnityEngine;
using System.Collections;
using System;

public enum MinionState
{
    stop,
    moving,
    fighting
}

public class Minions : MonoBehaviour
{
    public MinionsInformations minionsInformations;
    public int playerNumber;
    public bool DEBUGFightInitator = false;

    public MinionState state { get; protected set; }
    public int ownerNumber { get; private set; }
    
    public Minions opponent { get; private set; }

    private NavMeshAgent navAgent;
    private Transform goal;
    private bool isInit = false;
    private int currentLife;
    private GameObject lifeBar;
    
    // Use this for initialization
    protected void Start () {
        initalize();
    }
	
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

	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Minion")
        {
            if (other.GetComponent<Minions>().opponent != this)
            {
                LaunchFight(other.GetComponent<Minions>());
            }
        }
    }

    public void SetOwnerNumber(int owner)
    {
        if(!isInit)
        {
            initalize();
        }

        ownerNumber = owner;
        
        SetGoal(GameObject.Find("Player" + ((ownerNumber%playerNumber) + 1)).transform);
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
                                                   (minionsInformations.baseLifePoints - currentLife) / minionsInformations.baseLifePoints);
    }


    public void Die()
    {

    }
}
