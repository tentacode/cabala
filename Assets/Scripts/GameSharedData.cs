using UnityEngine;
using System.Collections;

public class GameSharedData : MonoBehaviour
{
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
