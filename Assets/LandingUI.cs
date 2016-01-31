using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LandingUI : MonoBehaviour
{
    public GameObject landingUI;
    public GameObject creditsUI;

    public MonoSoundable sound;

    bool isCredits = false;

    public void SwitchToCredits()
    {
        isCredits = true;
        sound.playSound();
    }

    public void BackToLanding()
    {
        isCredits = false;
        sound.playSound();
    }

    public void SwitchToPlay()
    {
        SceneManager.LoadScene("Lobby");
        sound.playSound();
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
