using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems;

public class MechanicsMenu : MonoBehaviour {
    public bool debugMode = false;
    public TMP_Text title;
    public TMP_Text description;
    public Mechanics mechanics;
    public VideoPlayer videoPlayer;
    public RawImage rawImage;
    public GameObject buttonObjects;


    private MechanicButton[] buttons;
    private PlayerFSM player;
    private Menu menu;

    private void Awake() {
        CleanInfo();
        buttons = buttonObjects.GetComponentsInChildren<MechanicButton>();
        player = GameObject.FindObjectOfType<PlayerFSM>();
        menu = GameObject.FindObjectOfType<Menu>();
    }

    private void Update() {
        CheckMenuInput();
    }

    private void OnEnable() {
        mechanics.SaveState();
    }

    void CleanInfo() {
        rawImage.enabled = false;
        title.text = "";
        description.text = "";
    }

    public void ClearEnabledMechanics() {
        mechanics.ResetMechanics();
        mechanics.EnableBasicMechanics();
        foreach (var button in buttons) {
            button.DeactivateButtonImage();
        }
    }

    public void ConfirmMechanics() {
        // check if all skill points were spent
        gameObject.SetActive(false);
        menu.pauseMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(menu.mechanicsButton);
        if (!debugMode) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void RevertMechanics() {
        mechanics.RestoreState();
        foreach (var button in buttons) {
            if (mechanics.IsEnabled(button.mechanicName)) {
                button.ActivateButtonImage();
            }
            else {
                button.DeactivateButtonImage();
            }
        }
    }

    void CheckMenuInput() {
        if (Input.GetButtonDown("Esc")) {
            RevertMechanics();
            ConfirmMechanics();
        }
    }
}
