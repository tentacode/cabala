using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class InvocationCircleControler : NetworkBehaviour
{
    [SerializeField]
    public SpawnerController _spawner;

    [SerializeField]
    private Unit_ID _unitID;

    private int Life
    {
        get
        {
            return _unitID.GetComponent<PlayerAuthorityScript>().cultisteLife;
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

    bool DeadIsAquired = false;

    public int CountPLayerDead()
    {
        int count = 0;
        foreach (var p in GameSharedData.GetAllPlayers)
        {
            if (!p.GetComponent<SpawnerController>().ISActive)
            {
                count++;
            }
        }
        return count;
    }
    float timeToWait = 10;
    float t = 0;
    void Update()
    {
        // Just draw Cultitsts
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
        
        if (t < timeToWait)
        {
            t += Time.deltaTime;
            return;
        }

        Debug.Log("Player dead : " + CountPLayerDead() + " " + GameSharedData.NumberOfPlayer);
        if (_unitID.isLocalPlayer && !DeadIsAquired && Life <= 0)
        {
            DeadIsAquired = true;
            Debug.Log("Dead " + CountPLayerDead() + " " + GameSharedData.NumberOfPlayer);

            GameObject.Find("GameOverEffects").GetComponent<UIDeath>().Activate(false);
            
        }
        else if (_unitID.isLocalPlayer && !DeadIsAquired && CountPLayerDead() >= GameSharedData.NumberOfPlayer - 1)
        {

            Debug.Log("Win " + CountPLayerDead() + " " + GameSharedData.NumberOfPlayer);

            DeadIsAquired = true;
             GameObject.Find("GameOverEffects").GetComponent<UIDeath>().Activate(true);
        }

        
    }

    public void CultistDeath(string minionName)
    {
        if (Life <= 0)
        {
            Lose();
        }
    }

    public void Lose()
    {
        _spawner.CmdisActive(false);
    }
}
