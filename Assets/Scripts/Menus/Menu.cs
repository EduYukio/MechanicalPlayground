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
    }

    public void RestartButton() {
        Manager.audio.mixer.SetFloat("bgmVolume", 0f);
        Time.timeScale = 1f;
        player.freezePlayerState = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
        if (hitEsc || hitStart) {
            if (pauseMenu.activeSelf) {
                ClosePauseMenu();
            }
            else if (mechanicsMenu == null || !mechanicsMenu.activeSelf) {
                OpenPauseMenu();
            }
        }

        if (hitCircle) {
            if (pauseMenu.activeSelf) {
                ClosePauseMenu();
            }
        }
    }

    void CheckSkillControlsInput() {
        if (Input.GetButtonDown("Select")) {
            if (pauseMenu.activeSelf) return;
            if (mechanicsMenu && mechanicsMenu.activeSelf) return;
            if (IsHidingUI()) return;

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
        Manager.audio.mixer.SetFloat("bgmVolume", -12.5f);

        Time.timeScale = 0f;
        player.freezePlayerState = true;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(continueButton);
    }

    IEnumerator WaitToClosePauseMenu() {
        pauseMenu.SetActive(false);
        Manager.audio.mixer.SetFloat("bgmVolume", 0f);
        yield return new WaitForSecondsRealtime(0.05f);
        Time.timeScale = 1f;
        player.freezePlayerState = false;
        Cursor.visible = false;
    }

    public void PlayUISound(string soundName) {
        Manager.audio.Play("UI_" + soundName);
    }
}
