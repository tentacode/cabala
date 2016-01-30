using UnityEngine;
using System.Collections;

public class GameSharedData : MonoBehaviour
{
    public int playerNumber;

    public bool autoCountPlayers;

    void Start()
    {
        if(autoCountPlayers)
        {
            playerNumber = GameObject.FindGameObjectsWithTag("Player").Length;
        }
    }
}
