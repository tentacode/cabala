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
    private int SideNumber;

    [HideInInspector]
	[SyncVar] public string my_ID ;
	private Transform myTransform;

    public int GetSideNumber()
    {
        return SideNumber;
    }

    /// <summary>
    /// Has to be called on the server side !
    /// </summary>
    /// <param name="side"></param>
    [Command]
    public void CmdSetSideId(int side)
    {
        SideNumber = side;
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
        myTransform.name = my_ID;
	}

    void LateUpdate()
    {
        // Change color regarding his side
        GetComponent<Renderer>().material.color = _colors[SideNumber];
    }
}
