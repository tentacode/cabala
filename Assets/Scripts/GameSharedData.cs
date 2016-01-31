using UnityEngine;
using System.Collections;

public class GameSharedData : MonoBehaviour
{
    public int NumberOfPlayer
    {
        get
        {
            return GameObject.FindGameObjectsWithTag("Player").Length;
        }
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
