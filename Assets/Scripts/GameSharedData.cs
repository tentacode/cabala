using UnityEngine;
using System.Collections;

public class GameSharedData : MonoBehaviour
{

    static GameObject localPlayer;

    public static GameObject GetLocalPlayer()
    {
        GameObject tofind;
        if (Camera.main.transform.position.x > 0 && Camera.main.transform.position.z > 0)
        {
            tofind = Unit_ID.FindPlayer(2);
        }
        else if (Camera.main.transform.position.x < 0 && Camera.main.transform.position.z > 0)
        {
            tofind = Unit_ID.FindPlayer(3);
        }
        else if (Camera.main.transform.position.x > 0 && Camera.main.transform.position.z < 0)
        {
            tofind = Unit_ID.FindPlayer(1);
        }
        else //if (Camera.main.transform.position.x < 0 && Camera.main.transform.position.y < 0)
        {
            tofind = Unit_ID.FindPlayer(4);
        }

        return tofind;
        /*
        if (localPlayer)
        {
            return localPlayer;
        }

        foreach (GameObject player in GameSharedData.GetAllPlayers)
        {
            if (player.GetComponent<PlayerNetwork>().connectionId == LANLobbyNetworkManager.singleton.client.connection.connectionId)
            {
                localPlayer = player;
            }
        }

        return localPlayer;*/
    }

    public static GameObject[] GetAllPlayers
    {
        get
        {
            return GameObject.FindGameObjectsWithTag("Player");
        }
    }

    public static int NumberOfPlayer
    {
        get
        {
            return GetAllPlayers.Length;
        }
    }

    public static GameObject GetPlayerNumberNext(Unit_ID playerInit, int numberToIncrement)
    {
        // Avoid overlaping yourself
        numberToIncrement = Mathf.Clamp(numberToIncrement, 0, GameSharedData.NumberOfPlayer - 1);

        Debug.Log(numberToIncrement);
        int pos = 0;
        // Find your position in the table
        foreach (var v in GetAllPlayers)
        {
            if (v.GetComponent<Unit_ID>().GetPlayerIndex() == playerInit.GetPlayerIndex())
            {
                break;
            }
            pos++;
        }

        Debug.Log(pos);

        int playerId = (pos + numberToIncrement) % (GameSharedData.NumberOfPlayer);

        Debug.Log(playerId);

        return GetAllPlayers[playerId];
    }
    /*
    public bool autoCountPlayers;

    void Start()
    {
        if(autoCountPlayers)
        {
            NumberOfPlayer = 
        }
    }*/
}
