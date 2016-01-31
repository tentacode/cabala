using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIDeath : MonoBehaviour {

    [SerializeField]
    private GameObject[] _objectLose;
    [SerializeField]
    private GameObject[] _objectWin;

    void Start()
    {
        Deactivate();
    }

    public void Activate(bool isWin)
    {
        if (isWin)
        {
            foreach (var v in _objectWin)
            {
                v.SetActive(true);
            }
        }
        else
        {
            foreach (var v in _objectLose)
            {
                v.SetActive(true);
            }
        }
    }

    public void Deactivate()
    {
        foreach (var v in _objectLose)
        {
            v.SetActive(false);
        }
        foreach (var v in _objectWin)
        {
            v.SetActive(false);
        }
    }

    public void ButtonGameAgain()
    {
        Deactivate();
        GameSharedData.GetLocalPlayer().GetComponent<PlayerAuthorityScript>().CmdRematch();
    }

    public void ButtonGoLobby()
    {
        GameObject.Find("NetworkLobbyManager").GetComponent<LANLobbyNetworkManager>().SendReturnToLobby();
    }
}
