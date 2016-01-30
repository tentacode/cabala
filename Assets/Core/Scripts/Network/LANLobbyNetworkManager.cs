using System;
using UnityEngine;
using UnityEngine.Networking;

public class LANLobbyNetworkManager : NetworkLobbyManager
{
    public void TogglePlayerReady()
    {
        var networkLobbyPlayer = GetCurrentNetworkLobbyPlayer();
        if (networkLobbyPlayer == null) {
            return;
        }
        
        if (networkLobbyPlayer.readyToBegin) {
            networkLobbyPlayer.SendNotReadyToBeginMessage();
        } else {
            networkLobbyPlayer.SendReadyToBeginMessage();
        }
        
        CheckReadyToBegin();
    }
    
    public void QuitLobby()
    {
        if (GetCurrentNetworkLobbyPlayer().isServer) {
            StopHost();
        } else {
            StopClient();
        }
        
        GetComponent<LANNetworkDiscovery>().QuitLobby();
    }
    
    public bool IsLocalPlayerReady()
    {
        var networkLobbyPlayer = GetCurrentNetworkLobbyPlayer();
        if (networkLobbyPlayer == null) {
            return false;
        }
        
        return networkLobbyPlayer.readyToBegin;
    }
    
    NetworkLobbyPlayer GetCurrentNetworkLobbyPlayer()
    {        
        return Array.Find(lobbySlots, slot => slot && slot.isLocalPlayer);
    }
    
    public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
    {   
        Debug.Log("OnLobbyServerSceneLoadedForPlayer");
        
        var playerIndex = lobbyPlayer.GetComponent<NetworkLobbyPlayer>().slot + 1;
        
        gamePlayer.name = "PLayer" + playerIndex;
        // gamePlayer.GetComponent<PlayerNetwork>.SetPlayerIndex(playerIndex);
        
        return true;
    }
    
    public override void OnLobbyClientExit()
    {
        base.OnLobbyClientExit();
        
        Debug.Log("OnLobbyClientExit");
    }
    
    public override void OnLobbyClientDisconnect(NetworkConnection conn)
    {
        base.OnLobbyClientDisconnect(conn);
        
        StopClient(); // ?
        GetComponent<LANNetworkDiscovery>().QuitLobby();
        Debug.Log("Leaving Lobby because disconnected");
    }
    
    public override void OnLobbyClientSceneChanged(NetworkConnection conn)
    {
        base.OnLobbyClientSceneChanged(conn);
        
        var lobbyPlayers = GameObject.FindGameObjectsWithTag("LobbyPlayer");
        foreach (GameObject go in lobbyPlayers) {
            go.GetComponent<LobbyPlayer>().Hide();
        }
        
        Debug.Log("OnLobbyClientSceneChanged");
    }
    
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        
        Debug.Log("Connect");
        // ClientScene.Ready (conn);
        // ClientScene.AddPlayer (0);
}
    
	// public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
	// {
    //     Debug.Log("Player added");
        
	// 	base.OnServerAddPlayer(conn, playerControllerId, extraMessageReader);
	// }
    
    // public override void OnLobbyClientAddPlayerFailed()
    // {
    //     Debug.Log("OnLobbyClientSceneChanged");
    // }
    
    // public override void OnClientError(NetworkConnection conn, int errorCode)
    // {
    //     Debug.Log("OnClientError");
    // }
    
    // public override void OnLobbyClientConnect(NetworkConnection conn)
    // {
    //     Debug.Log("OnLobbyClientConnect");
    // }
    
    // public override void OnLobbyClientEnter()
    // {
    //     Debug.Log("OnLobbyClientEnter");
    // }
    
    // public override void OnLobbyStartClient(NetworkClient client)
    // {
    //     Debug.Log("OnLobbyStartClient");
    // }
    
    // public override void OnLobbyStopClient()
    // {
    //     Debug.Log("OnLobbyStopClient");
    // }
}
