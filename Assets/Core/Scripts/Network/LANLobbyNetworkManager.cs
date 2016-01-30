using System;
using UnityEngine;
using UnityEngine.Networking;

public class LANLobbyNetworkManager : NetworkLobbyManager
{
    GameObject localPlayer;
    
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
        
        NetworkLobbyPlayer nlp = lobbyPlayer.GetComponent<NetworkLobbyPlayer>();
        var playerIndex = nlp.slot + 1;
        
        PlayerNetwork pn = gamePlayer.GetComponent<PlayerNetwork>();
        pn.playerIndex = playerIndex;
        pn.connectionId = nlp.connectionToClient.connectionId;
        
        return true;
    }
    
    GameObject GetLocalPlayer()
    {
        if (localPlayer) {
            return localPlayer;
        }
        
        var players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players) {
            if (player.GetComponent<PlayerNetwork>().connectionId == client.connection.connectionId) {
                localPlayer = player;
            }
        }
        
        return localPlayer;
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
        Debug.Log("OnLobbyClientSceneChanged");
          
        var lobbyPlayers = GameObject.FindGameObjectsWithTag("LobbyPlayer");
        foreach (GameObject go in lobbyPlayers) {
            go.GetComponent<LobbyPlayer>().Hide();
        }
        
        GetLocalPlayer().GetComponent<PlayerNetwork>().InitGame();
    }
}
