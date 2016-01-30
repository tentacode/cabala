using UnityEngine;
using System.Collections;
using System;

public class Ennemy : MonoBehaviour
{
    public int ownerNumber { get; private set; }
    public int playerNumber;

    private NavMeshAgent navAgent;
    private Transform goal;

    private bool isInit = false;

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

        if (playerNumber <= 0)
        {
            playerNumber = GameObject.Find("GameSharedData").GetComponent<GameSharedData>().PlayerNumber;
        }
    }

	// Update is called once per frame
	void Update () {
	
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
}
