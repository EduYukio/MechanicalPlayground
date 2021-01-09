using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    public PreloadScript preload;

    private void Start() {
        preload = GameObject.Find("PreloadObject").GetComponent<PreloadScript>();
    }

    public void PlayButton() {
        if (preload != null && preload.currentBGM.source == null) {
            preload.InitializeFirstBGM();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void CreditsButton() {
        SceneManager.LoadScene("Credits");
    }

    public void QuitButton() {
        Application.Quit();
    }
}
