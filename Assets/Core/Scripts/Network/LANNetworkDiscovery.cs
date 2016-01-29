using UnityEngine;
using UnityEngine.Networking;

public enum PlayerLobbyState
{
    Discovering,
    GameFound,
    InLobby
}

public class LANNetworkDiscovery : NetworkDiscovery
{
    LANLobbyNetworkManager networkManager;
    private PlayerLobbyState playerLobbyState;
    private string gameIp;
    
    public PlayerLobbyState PlayerLobbyState
    {
        get { return playerLobbyState; }
    }

    void Start()
    {
        networkManager = GetComponent<LANLobbyNetworkManager>();

        StartDiscovering();
    }

    public void StartDiscovering()
    {
        playerLobbyState = PlayerLobbyState.Discovering;
        Initialize();
        StartAsClient();
    }

    public void HostGame()
    {
        playerLobbyState = PlayerLobbyState.InLobby;
        StopBroadcast();
        StartAsServer();

        networkManager.networkAddress = Network.player.ipAddress;
        networkManager.StartHost();
    }

    public void JoinGame()
    {
        playerLobbyState = PlayerLobbyState.InLobby;
        StopBroadcast();
        NetworkTransport.Shutdown();

        networkManager.networkAddress = gameIp;
        networkManager.StartClient();
    }
    
    public void QuitLobby()
    {
        if (isServer) {
            StopBroadcast();
        }
        
        StartDiscovering();
    }

    override public void OnReceivedBroadcast(string fromAddress, string data)
    {
        playerLobbyState = PlayerLobbyState.GameFound;
        gameIp = fromAddress;
    }
}
