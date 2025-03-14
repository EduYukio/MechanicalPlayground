﻿using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems;
using System;

public class MechanicsMenu : MonoBehaviour
{
    public TMP_Text title;
    public TMP_Text description;
    public Mechanics mechanics;
    public VideoPlayer videoPlayer;
    public RawImage rawImage;
    public GameObject buttonObjects;
    public GameObject confirmationPopup;
    public GameObject popupSaveButton;
    public bool changedMechanics = false;
    public TMP_Text skillPointsText;
    public int skillPoints;

    [HideInInspector] public MechanicButton[] buttons;
    private PlayerFSM player;
    private Menu menu;
    private bool blinkRed;

    private void Awake()
    {
        CleanInfo();
        buttons = buttonObjects.GetComponentsInChildren<MechanicButton>();
        player = GameObject.FindObjectOfType<PlayerFSM>();
        menu = GameObject.FindObjectOfType<Menu>();
        skillPoints = player.config.maxSkillPoints;
        skillPointsText.color = Color.black;
    }

    private void Update()
    {
        CheckMenuExitInput();
        Helper.CheckIfNeedToBlinkRed(ref blinkRed, skillPointsText);
    }

    private void OnEnable()
    {
        mechanics.SaveState();
        changedMechanics = false;
        buttons = buttonObjects.GetComponentsInChildren<MechanicButton>();
        UpdateButtonsState();
    }

    private void CleanInfo()
    {
        rawImage.enabled = false;
        title.text = "";
        description.text = "";
    }

    public void ClearEnabledMechanics()
    {
        mechanics.ResetMechanics();
        mechanics.EnableBasicMechanics();
        foreach (var button in buttons)
        {
            button.DeactivateButtonImage();
        }
        changedMechanics = true;
        UpdateButtonsState();
    }

    public void ConfirmMechanics()
    {
        if (skillPoints > 0)
        {
            SkillPointsWarning();
            return;
        }

        Manager.audio.Play("UI_Confirm");
        if (changedMechanics)
        {
            ReloadLevel();
        }
        else
        {
            BackToPauseMenu();
        }
    }

    public void ReloadLevel()
    {
        Manager.audio.SetBGMVolumeToNormal();
        Time.timeScale = 1f;
        player.freezePlayerState = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToPauseMenu()
    {
        gameObject.SetActive(false);
        menu.pauseMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(menu.mechanicsButton);
    }

    public void OpenConfirmationPopup()
    {
        confirmationPopup.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(popupSaveButton);
    }

    public void RevertMechanics()
    {
        changedMechanics = false;
        mechanics.RestoreState();
        foreach (var button in buttons)
        {
            if (mechanics.IsEnabled(button.mechanicName))
            {
                button.ActivateButtonImage();
            }
            else
            {
                button.DeactivateButtonImage();
            }
        }
        UpdateButtonsState();
    }

    private void CheckMenuExitInput()
    {
        bool hitEsc = Input.GetButtonDown("Esc");
        bool hitStart = Input.GetButtonDown("Start");
        bool hitCircle = Input.GetButtonDown("Circle");
        if (hitEsc || hitStart || hitCircle)
        {
            if (skillPoints > 0)
            {
                SkillPointsWarning();
                return;
            }

            ConfirmMechanics();
        }
    }

    public void SkillPointsWarning()
    {
        skillPointsText.color = Color.red;
        blinkRed = true;
        Manager.audio.Play("UI_Fail");
    }

    public void UpdateSkillPointsText()
    {
        skillPoints = player.config.maxSkillPoints;
        foreach (var button in buttons)
        {
            if (mechanics.IsEnabled(button.mechanicName))
            {
                skillPoints--;
            }
        }
        skillPointsText.text = "Skill Points: " + Convert.ToString(skillPoints);
    }

    public void UpdateButtonsState()
    {
        foreach (var button in buttons)
        {
            button.DecideIfIsBlocked();
            button.SetButtonAppearance();
            UpdateSkillPointsText();
        }
    }
}
