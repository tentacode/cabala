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
    }

    public override void OnLobbyClientConnect(NetworkConnection conn)
    {
        Debug.Log("Enters Lobby");
        Debug.Log(conn.connectionId);

        GameObject schema = GameObject.Find("Schema");
        GameObject cameraTarget = GameObject.Find("CameraTarget");

        switch (conn.connectionId) {
            case 0:
                schema.transform.rotation = Quaternion.Euler(new Vector3(0,-90,0));
                cameraTarget.transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
                break;
            case 1:
                schema.transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
                cameraTarget.transform.rotation = Quaternion.Euler(new Vector3(0,-90,0));
                break;
            case 2:
                schema.transform.rotation = Quaternion.Euler(new Vector3(0,90,0));
                cameraTarget.transform.rotation = Quaternion.Euler(new Vector3(0,180,0));
                break;
            case 3:
                schema.transform.rotation = Quaternion.Euler(new Vector3(0,180,0));
                cameraTarget.transform.rotation = Quaternion.Euler(new Vector3(0,90,0));
                break;
        }
    }
    
    public override void OnLobbyServerConnect(NetworkConnection conn)
    {        
        int playerConnected = 0;
        foreach (NetworkLobbyPlayer slot in lobbySlots) {
            if (slot) {
                playerConnected++;
            }
        }
        
        minPlayers = playerConnected + 1;
        if (minPlayers < 2) {
            minPlayers = 2;
        }
    }
    
    public override void OnLobbyServerDisconnect(NetworkConnection conn)
    {        
        int playerConnected = 0;
        foreach (NetworkLobbyPlayer slot in lobbySlots) {
            if (slot) {
                playerConnected++;
            }
        }
        
        minPlayers = playerConnected;
        if (minPlayers < 2) {
            minPlayers = 2;
        }
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
        pn.GetComponent<Unit_ID>().CmdSetPlayerIndex(playerIndex);
        pn.GetComponent<Unit_ID>().CmdSetMyUniqueID("Player" + playerIndex);
        pn.connectionId = nlp.connectionToClient.connectionId;
        
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
        Debug.Log("OnLobbyClientSceneChanged");
        GetComponent<LANNetworkDiscovery>().GameStarted();
          
        var lobbyPlayers = GameObject.FindGameObjectsWithTag("LobbyPlayer");
        foreach (GameObject go in lobbyPlayers) {
            go.GetComponent<LobbyPlayer>().Hide();
        }
    }
}
