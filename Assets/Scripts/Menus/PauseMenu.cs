using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    public GameObject mechanicsMenu;
    private PlayerFSM player;

    private void Awake() {
        player = GameObject.FindObjectOfType<PlayerFSM>();
    }

    private void OnEnable() {
        Time.timeScale = 0f;
        player.freezePlayerState = true;
    }

    private void OnDisable() {
        Time.timeScale = 1f;
        player.freezePlayerState = false;
    }

    private void Update() {
        CheckMenuInput();
    }



    public void ContinueButton() {
        gameObject.SetActive(false);
    }

    public void MechanicsButton() {
        gameObject.SetActive(false);
        mechanicsMenu.SetActive(true);
    }

    public void OptionsButton() {
    }

    public void QuitToMenuButton() {
        OnDisable();
        PlayerFSM.respawnPosition = Vector2.zero;
        SceneManager.LoadScene("MainMenu");
    }

    void CheckMenuInput() {
        if (Input.GetButtonDown("Esc")) {
            gameObject.SetActive(false);
        }
    }
}
