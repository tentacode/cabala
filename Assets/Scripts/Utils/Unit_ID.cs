using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Unit_ID : NetworkBehaviour {

    Color[] _colors =
    {
        Color.black,
        Color.white,
        Color.blue,
        Color.green
    };

    // Side 0 is neutral
    [SerializeField]
    [SyncVar]
    private int PlayerIndex;

    public static GameObject FindPlayer(int playerId)
    {
        return GameObject.Find("Player" + (playerId).ToString());
    }

    [HideInInspector]
	[SyncVar] private string my_uniqueID ;
	private Transform myTransform;

    public bool IsReady()
    {
        return my_uniqueID != "";
    }

    public int GetPlayerIndex()
    {
        return PlayerIndex;
    }

    /// <summary>
    /// Has to be called on the server side !
    /// </summary>
    /// <param name="number"></param>
    [Command]
    public void CmdSetPlayerIndex(int number)
    {
        PlayerIndex = number;
    }

    [Command]
    public void CmdSetMyUniqueID(string id)
    {
        my_uniqueID = id;
    }

	// Use this for initialization
	void Start () 
	{
		myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
        // Set Identity
        myTransform.name = my_uniqueID;
	}

    void LateUpdate()
    {
        // Change color regarding his side
      //  GetComponent<Renderer>().material.color = _colors[PlayerNumber];
    }
}
