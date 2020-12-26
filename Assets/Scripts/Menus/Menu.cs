using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {
    public GameObject pauseMenu;
    public GameObject mechanicsMenu;

    public GameObject continueButton;
    public GameObject mechanicsButton;
    public GameObject optionsButton;

    public GameObject firstMechanicButton;

    private PlayerFSM player;

    private void Awake() {
        player = GameObject.FindObjectOfType<PlayerFSM>();
    }

    void Update() {
        CheckMenuInput();
    }

    public void ContinueButton() {
        ClosePauseMenu();
    }

    public void MechanicsButton() {
        pauseMenu.SetActive(false);
        mechanicsMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstMechanicButton);
    }

    public void OptionsButton() {
    }

    public void QuitToMenuButton() {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        player.freezePlayerState = false;
        PlayerFSM.respawnPosition = Vector2.zero;
        SceneManager.LoadScene("MainMenu");
    }

    void CheckMenuInput() {
        if (Input.GetButtonDown("Esc")) {
            if (pauseMenu.activeSelf) {
                ClosePauseMenu();
            }
            else {
                OpenPauseMenu();
            }
        }
        else if (Input.GetButtonDown("Circle") && pauseMenu.activeSelf) {
            ClosePauseMenu();
        }
    }

    void ClosePauseMenu() {
        StartCoroutine(WaitToClosePauseMenu());
    }

    void OpenPauseMenu() {
        pauseMenu.SetActive(true);

        Time.timeScale = 0f;
        player.freezePlayerState = true;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(continueButton);
    }

    IEnumerator WaitToClosePauseMenu() {
        pauseMenu.SetActive(false);
        yield return new WaitForSecondsRealtime(0.05f);
        Time.timeScale = 1f;
        player.freezePlayerState = false;
    }

}
