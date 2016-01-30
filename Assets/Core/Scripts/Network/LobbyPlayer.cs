using UnityEngine;
using UnityEngine.Networking;

public class LobbyPlayer : MonoBehaviour
{
    public GameObject readyFlag;
    
    NetworkLobbyPlayer networkLobbyPlayer;
    
    public void Hide()
    {
        GetComponent<Renderer>().enabled = false;
        readyFlag.GetComponent<Renderer>().enabled = false;
    }

    void Start ()
    {
        networkLobbyPlayer = GetComponent<NetworkLobbyPlayer>();
        
        switch(networkLobbyPlayer.slot) {
            case 0:
                transform.position = new Vector3(-2,0.5f,2);
                GetComponent<Renderer>().material.color = Color.red;
                break;
            case 1:
                transform.position = new Vector3(2,0.5f,2);
                GetComponent<Renderer>().material.color = Color.green;
                break;
            case 2:
                transform.position = new Vector3(2,0.5f,-2);
                GetComponent<Renderer>().material.color = Color.blue;
                break;
            case 3:
                transform.position = new Vector3(-2,0.5f,-2);
                GetComponent<Renderer>().material.color = Color.yellow;
                break;
        }
    }
    
    void Update()
    {
        readyFlag.SetActive(networkLobbyPlayer.readyToBegin);
    }
}
