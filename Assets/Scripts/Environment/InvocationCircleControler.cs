using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class InvocationCircleControler : NetworkBehaviour
{
    [SerializeField]
    private SpawnerController _spawner;

    [SerializeField]
    private Unit_ID _unitID;

    private int Life
    {
        get
        {
            return _unitID.GetComponent<PlayerAuthorityScript>().cultisteLife;
        }
    }

    [HideInInspector]
    public int LlifePoints
    {
        get
        {
            return AllCultists.Count;
        }
    }

    public List<Cultist> AllCultists;
    public bool autoFillCultists;

    // Use this for initialization
    void Awake () {

	    if(autoFillCultists)
        {
            AllCultists = new List<Cultist>();
            GameObject cultistsParents = transform.Find("Cultists").gameObject;

            for (int i = 0; i < cultistsParents.transform.childCount; i++)
            {
                
                Cultist currentCultist = cultistsParents.transform.GetChild(i).GetComponent<Cultist>();
                AllCultists.Add(currentCultist);
             //   currentCultist.init(this);
            }

        }
	}

    void OnTriggerEnter(Collider other)
    {
   
        if (!_unitID.isLocalPlayer)
        {
            return;
        }

        if (Life <= 0)
        {
            return;
        }

        Debug.Log("Invoc enter !");

        if (other.tag == "Minion")
        {
            if (other.GetComponent<Unit_ID>().GetPlayerIndex() != _unitID.GetPlayerIndex())
            {
                //CmdCultistDeath(name);

              //  cultistDeath(name);

                Debug.Log("Do my job !");
                _unitID.GetComponent<PlayerAuthorityScript>().CmdDestroyCultiste(other.name);
            }
        }
    }

    void Update()
    {
        int count = 0;
        foreach (var c in AllCultists)
        {
            if (count < Life)
            {
                c.gameObject.SetActive(true);
            }
            else
            {
                c.gameObject.SetActive(false);
            }
            count++;
        }

        if (Life == 0)
        {
            enabled = false;
        }

    }

    public void CultistDeath(string minionName)
    {
        Debug.Log("Life--");

        
        /*
        AllCultists.RemoveAt(0);
        AllCultists[0].CmdSetIsActive(false);*/
        

       // Destroy( cultistsLeft[0].gameObject);
       // NetworkServer.UnSpawn(cultistsLeft[0].gameObject);


        if (Life <= 0)
        {
            Lose();
        }
    }
    static int PlayerDead;

    public void Lose()
    {
        PlayerDead++;

        _spawner.CmdisActive( false );

        if (PlayerDead >= GameSharedData.NumberOfPlayer - 1)
        {
            // Game OVER
            _unitID.GetComponent<PlayerAuthorityScript>().CmdRematch();
        }

        // do stuff
    }
}
