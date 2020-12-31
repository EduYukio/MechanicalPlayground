using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public TMP_Text levelIndicator;
    private const int finalLevel = 15;

    private void Awake() {
        player = GameObject.FindObjectOfType<PlayerFSM>();
    }

    private void Start() {
        string currentLevelText = SceneManager.GetActiveScene().buildIndex.ToString();
        levelIndicator.text = "Level: " + currentLevelText + "/" + finalLevel.ToString();
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
        bool hitEsc = Input.GetButtonDown("Esc");
        bool hitStart = Input.GetButtonDown("Start");
        bool hitCircle = Input.GetButtonDown("Circle");
        if (hitEsc || hitStart || hitCircle) {
            if (pauseMenu.activeSelf) {
                ClosePauseMenu();
            }
            else if (!mechanicsMenu.activeSelf) {
                OpenPauseMenu();
            }
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
