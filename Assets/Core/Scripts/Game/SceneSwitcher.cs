using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void SwitchToLobby()
    {
        SceneManager.LoadScene("Lobby");
    }
}
