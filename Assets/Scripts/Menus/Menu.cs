using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {
    private PlayerFSM player;

    public GameObject pauseMenu;
    public GameObject mechanicsMenu;

    public GameObject optionsMenu;
    public GameObject firstOptionsButton;

    public GameObject continueButton;
    public GameObject mechanicsButton;
    public GameObject optionsButton;

    public GameObject firstMechanicButton;

    public GameObject skillUICompactChildAggregator;
    public GameObject skillUILargeChildAggregator;

    public TMP_Text levelIndicator;
    private const int finalLevel = 15;

    private void Awake() {
        player = GameObject.FindObjectOfType<PlayerFSM>();
        Cursor.visible = false;
    }

    private void Start() {
        string currentLevelText = (SceneManager.GetActiveScene().buildIndex - 1).ToString();
        levelIndicator.text = "Level: " + currentLevelText + "/" + finalLevel.ToString();
    }

    void Update() {
        CheckMenuInput();
        CheckSkillControlsInput();
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
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstOptionsButton);
    }

    public void RestartButton() {
        Manager.audio.SetBGMVolumeToNormal();
        Time.timeScale = 1f;
        player.freezePlayerState = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitToMenuButton() {
        Manager.audio.SetBGMVolumeToNormal();
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        player.freezePlayerState = false;
        PlayerFSM.respawnPosition = Vector2.zero;
        SceneManager.LoadScene("MainMenu");
    }

    public void BackToPauseMenu() {
        Manager.audio.SetBGMVolumeToMuffled();
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsButton);
    }

    void CheckMenuInput() {
        bool hitEsc = Input.GetButtonDown("Esc");
        bool hitStart = Input.GetButtonDown("Start");
        bool hitCircle = Input.GetButtonDown("Circle");
        if (hitEsc || hitStart) {
            if (pauseMenu.activeSelf) {
                Manager.audio.Play("UI_Confirm");
                ClosePauseMenu();
            }
            else if (optionsMenu.activeSelf) {
                Manager.audio.Play("UI_Confirm");
                BackToPauseMenu();
            }
            else if (mechanicsMenu == null || !mechanicsMenu.activeSelf) {
                OpenPauseMenu();
            }
        }

        if (hitCircle) {
            if (pauseMenu.activeSelf) {
                Manager.audio.Play("UI_Confirm");
                ClosePauseMenu();
            }
            else if (optionsMenu.activeSelf) {
                Manager.audio.Play("UI_Confirm");
                BackToPauseMenu();
            }
        }
    }

    void CheckSkillControlsInput() {
        if (Input.GetButtonDown("Select")) {
            if (pauseMenu.activeSelf) return;
            if (mechanicsMenu && mechanicsMenu.activeSelf) return;
            if (IsHidingUI()) return;

            Manager.audio.Play("UI_Toggle");
            skillUICompactChildAggregator.SetActive(!skillUICompactChildAggregator.activeSelf);
            skillUILargeChildAggregator.SetActive(!skillUILargeChildAggregator.activeSelf);
        }
    }

    bool IsHidingUI() {
        if (!skillUICompactChildAggregator.activeSelf && !skillUILargeChildAggregator.activeSelf) return true;

        return false;
    }

    void ClosePauseMenu() {
        StartCoroutine(WaitToClosePauseMenu());
    }

    void OpenPauseMenu() {
        pauseMenu.SetActive(true);
        Manager.audio.SetBGMVolumeToMuffled();

        Time.timeScale = 0f;
        player.freezePlayerState = true;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(continueButton);
    }

    IEnumerator WaitToClosePauseMenu() {
        pauseMenu.SetActive(false);
        Manager.audio.SetBGMVolumeToNormal();
        yield return new WaitForSecondsRealtime(0.05f);
        Time.timeScale = 1f;
        player.freezePlayerState = false;
        Cursor.visible = false;
    }
}
