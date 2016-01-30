using UnityEngine;
using System.Collections;

public class Ennemy : MonoBehaviour
{
    public Transform goal;

    private NavMeshAgent navAgent;

	// Use this for initialization
	protected void Start () {
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.destination = goal.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
