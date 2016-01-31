using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class InvocationCircleControler : MonoBehaviour
{
    [SerializeField]
    private SpawnerController _spawner;

    [SerializeField]
    private Unit_ID _unitID;

    [HideInInspector]
    public int LlifePoints
    {
        get
        {
            return cultistsLeft.Count;
        }
    }
    public List<Cultist> AllCultists = new List<Cultist>();
    public List<Cultist> cultistsLeft;
    public bool autoFillCultists;

    // Use this for initialization
    void Start () {

	    if(autoFillCultists)
        {
            cultistsLeft = new List<Cultist>();
            GameObject cultistsParents = transform.Find("Cultists").gameObject;

            for (int i = 0; i < cultistsParents.transform.childCount; i++)
            {
                
                Cultist currentCultist = cultistsParents.transform.GetChild(i).GetComponent<Cultist>();
                AllCultists.Add(currentCultist);
                cultistsLeft.Add(currentCultist);
                currentCultist.init(this);
            }
        }
	}

    void OnEnable()
    {
        CultistDead = 0;
        cultistsLeft.Clear();
        foreach (var c in AllCultists)
        {
            c.gameObject.SetActive(true);
            cultistsLeft.Add(c);
        }
    }
	

    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Minion")
        {
            if (other.GetComponent<Unit_ID>().GetPlayerIndex() != _unitID.GetPlayerIndex())
            {
                launchMinionOnCultist(other.GetComponent<Minions>());
            }
        }
    }

    public void launchMinionOnCultist(Minions minion)
    {
        //Debug.Log("Launch Minion " + minion.name + " on cultist " + (lifePoints - 1));
        minion.SetGoal(cultistsLeft[LlifePoints - 1].transform);
    }

    public void cultistDeath()
    {
        cultistsLeft.RemoveAt(0);

        if(LlifePoints <= 0)
        {
            Lose();
        }
    }


    static int CultistDead;

    public void Lose()
    {
        CultistDead++;

        _spawner.enabled = false;

        if (CultistDead >= GameSharedData.NumberOfPlayer - 1)
        {
            Debug.Log("Game OVER !");
        }

        // do stuff
    }
}
