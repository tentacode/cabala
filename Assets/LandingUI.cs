using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LandingUI : MonoBehaviour
{
    public GameObject landingUI;
    public GameObject creditsUI;

    bool isCredits = false;

    public void SwitchToCredits()
    {
        isCredits = true;
    }

    public void BackToLanding()
    {
        isCredits = false;
    }

    public void SwitchToPlay()
    {
        SceneManager.LoadScene("Lobby");
    }

	void Update ()
    {
        if (isCredits) {
            landingUI.SetActive(false);
            creditsUI.SetActive(true);
        } else {
            creditsUI.SetActive(false);
            landingUI.SetActive(true);
        }
	}
}
