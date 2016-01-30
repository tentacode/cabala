using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

public class PoolManagerBase : NetworkBehaviour
{
    protected Stack<GameObject> pool = new Stack<GameObject>();

    [HideInInspector]
    public Transform root;

    public GameObject objectToInstantiate;

    public int numberToPreInstantiate;

    protected static int nextNameId = 0;

    public static PoolManagerBase FindPool(MinionType type)
    {
        return GameObject.Find("Pool" + type.ToString()).GetComponent<PoolManagerBase>();
    }

    protected virtual void Start()
    {
        root = transform;

        if (!isServer)
        {
            return;
        }

        for (int j = 0; j < numberToPreInstantiate; j++)
        {
            GameObject o = Generate();
            o.GetComponent<Destructible>().GoDeadNoBroadcart();

            pool.Push(o);

        }
    }


    protected virtual GameObject Generate()
    {
        if (!isServer)
        {
            Debug.LogError("Only server should generate new units !");
        }

        GameObject toPop = Instantiate(objectToInstantiate) as GameObject;

        toPop.SetActive(false);

        string uniqueID = toPop.name + " " + nextNameId;
        toPop.GetComponent<Unit_ID>().CmdSetMyUniqueID(uniqueID);
        Debug.Log("Generate " + uniqueID);

        nextNameId++;
        
        toPop.transform.parent = root;

        toPop.GetComponent<Destructible>().HandleDestroyed += OnDeath;

        return toPop;
    }

    public GameObject Pop()
    {
        GameObject toPop;

        try
        {
            toPop = pool.Pop();
        }
        catch (System.InvalidOperationException)
        {
            toPop = Generate();
        }

        toPop.GetComponent<Destructible>().GoAlive();

        return toPop;
    }

    public void OnDeath(GameObject deadObject)
    {
        pool.Push(deadObject);
    }
}
