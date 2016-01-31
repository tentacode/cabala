using UnityEngine;
using UnityEngine.UI;

public class LobbyMenu : MonoBehaviour
{   
    public GameObject joinMenu;
    public GameObject joinButton;
    public GameObject readyMenu;
    public Text readyText;
    public GameObject instructions;
    public GameObject schema;
    
    LANNetworkDiscovery networkDiscovery;
    LANLobbyNetworkManager lobbyNetworkManager;
    
    void Start ()
    {
        GameObject networkManager = GameObject.FindGameObjectWithTag("NetworkManager");
        networkDiscovery = networkManager.GetComponent<LANNetworkDiscovery>();
        lobbyNetworkManager = networkManager.GetComponent<LANLobbyNetworkManager>();
        
        SwitchToDiscovering();
    }
    
    public void JoinGame()
    {
        networkDiscovery.JoinGame();
    }
    
    public void HostGame()
    {
        networkDiscovery.HostGame();
    }
    
    public void ToggleReady()
    {
        lobbyNetworkManager.TogglePlayerReady();
    }
    
    public void QuitLobby()
    {
        lobbyNetworkManager.QuitLobby();
    }
    
    void Update()
    {
        switch (networkDiscovery.PlayerLobbyState)
        {
            case PlayerLobbyState.Discovering:
                SwitchToDiscovering();
                break;
            case PlayerLobbyState.GameFound:
                SwitchToGameFound();
                break;
            case PlayerLobbyState.InLobby:
                SwitchToLobby();
                break;
        }
    }
    
    void SwitchToDiscovering()
    {
        schema.SetActive(false);
        instructions.SetActive(false);
        joinMenu.SetActive(true);
        joinButton.SetActive(false);
        readyMenu.SetActive(false);
    }
    
    void SwitchToLobby()
    {
        schema.SetActive(true);
        instructions.SetActive(true);
        bool isReady = lobbyNetworkManager.IsLocalPlayerReady();
        readyText.text = isReady ? "I'm not ready" : "I'm ready !";
        joinMenu.SetActive(false);
        readyMenu.SetActive(true);
    }   
    
    void SwitchToGameFound()
    {
        schema.SetActive(false);
        instructions.SetActive(false);
        joinMenu.SetActive(true);
        joinButton.SetActive(true);
        readyMenu.SetActive(false);
    }
}
