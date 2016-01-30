using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

public class PoolManagerBase : MonoBehaviour
{
    protected Stack<GameObject> pool = new Stack<GameObject>();

    [HideInInspector]
    public Transform root;

    public GameObject objectToInstantiate;

    public int numberToPreInstantiate;

    protected int nextNameId = 0;

    protected virtual void Start()
    {
        root = transform;

        for (int j = 0; j < numberToPreInstantiate; j++)
        {
            GameObject o = Generate();
            o.GetComponent<Destructible>().GoDeadNoBroadcart();

            pool.Push(o);

        }
    }

    protected virtual GameObject Generate()
    {
        GameObject toPop = Instantiate(objectToInstantiate) as GameObject;

        toPop.name += " " + nextNameId;
        nextNameId++;

        toPop.transform.parent = root;

        toPop.GetComponent<Destructible>().HandleDestroyed += OnDeath;

        NetworkServer.Spawn(toPop);

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

    public void OnDeath(GameObject deadObject, Destructible attaking)
    {
        pool.Push(deadObject);
    }
}
