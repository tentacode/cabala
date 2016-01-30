using UnityEngine;
using System.Collections.Generic;

public class InvocationCircleControler : MonoBehaviour
{
    public int ownerIndex;

    public int lifePoints;
    
    public List<Cultist> cultists;
    public bool autoFillCultists;

    public List<SpawnerController> spawners;
    public bool autoFillSpawners;

    // Use this for initialization
    void Start () {
	    if(autoFillCultists)
        {
            cultists = new List<Cultist>();
            GameObject cultistsParents = transform.Find("Cultists").gameObject;

            lifePoints = cultistsParents.transform.childCount;

            for (int i = 0; i < cultistsParents.transform.childCount; i++)
            {
                Cultist currentCultist = cultistsParents.transform.GetChild(i).GetComponent<Cultist>();
                cultists.Add(currentCultist);
                currentCultist.init(this);
            }
        }

        if(autoFillSpawners)
        {
            spawners = new List<SpawnerController>();
            GameObject spawnerParents = transform.Find("Spawners").gameObject;

            for (int i = 0; i < spawnerParents.transform.childCount; i++)
            {
                SpawnerController currentSpawner = spawnerParents.transform.GetChild(i).GetComponent<SpawnerController>();
                spawners.Add(currentSpawner);
                currentSpawner.ownerIndex = ownerIndex;
                //spawners.init(this, i);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Minion")
        {
            if (other.GetComponent<Minions>().ownerIndex != ownerIndex)
            {
                launchMinionOnCultist(other.GetComponent<Minions>());
            }
        }
    }

    public void launchMinionOnCultist(Minions minion)
    {
        Debug.Log("Launch Minion " + minion.name + " on cultist " + (lifePoints - 1));
        minion.SetGoal(cultists[lifePoints - 1].transform);
    }

    public void cultistDeath()
    {
        lifePoints--;
        cultists.RemoveAt(lifePoints);

        if(lifePoints <= 0)
        {
            Lose();
        }
    }

    public void Lose()
    {
        // do stuff
    }
}
