using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public void PlayButton() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void CreditsButton() {
        SceneManager.LoadScene("Credits");
    }

    public void QuitButton() {
        Application.Quit();
    }

    public void CreditsBackButton() {
        SceneManager.LoadScene("MainMenu");
    }
}
