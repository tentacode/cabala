using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LandingUI : MonoBehaviour
{
    public GameObject landingUI;
    public GameObject creditsUI;

    public MonoSoundable sound;
    public MusicalMono menuMusic;
    public MusicalMono creditMusic;

    bool isCredits = false;

    public void SwitchToCredits()
    {
        isCredits = true;
        sound.playSound();
        creditMusic.playSound();
    }

    public void BackToLanding()
    {
        isCredits = false;
        sound.playSound();
        menuMusic.playSound();
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
