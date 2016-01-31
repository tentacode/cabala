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
    public int broadcastTimeout = 5;

    LANLobbyNetworkManager networkManager;
    private PlayerLobbyState playerLobbyState;
    private string gameIp;
    private float lastBroadcast;
    
    public PlayerLobbyState PlayerLobbyState
    {
        get { return playerLobbyState; }
    }

    void Start()
    {
        // Disable screen dimming
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        networkManager = GetComponent<LANLobbyNetworkManager>();

        StartDiscovering();
    }
    
    void LateUpdate()
    {
        if (isClient && lastBroadcast < Time.time - broadcastTimeout) {
            gameIp = null;
            playerLobbyState = PlayerLobbyState.Discovering;
        }
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
    
    public void GameStarted()
    {
        if (isServer) {
            StopBroadcast();
        }
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
        lastBroadcast = Time.time;
        playerLobbyState = PlayerLobbyState.GameFound;
        gameIp = fromAddress;
    }
}
