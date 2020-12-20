using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;
using UnityEngine.UI;

public class MechanicButton : MonoBehaviour {
    public VideoClip mechanicClip;
    public string mechanicName;
    public string description;
    public static MechanicsMenu mechMenu;

    public bool isBlocked = false;
    public bool isActive = false;

    private Image buttonImage;
    private Color activeColor = new Color(100 / 255f, 122 / 255f, 224 / 255f, 1f);
    private Color inactiveColor = new Color(160 / 255f, 160 / 255f, 160 / 255f, 1f);
    private Color blockedColor = new Color(160 / 255f, 160 / 255f, 160 / 255f, 0.5f);
    private Color blockedTextColor = new Color(1, 1, 1, 0.5f);

    private void Awake() {
        if (mechMenu == null) mechMenu = GameObject.FindObjectOfType<MechanicsMenu>();
        buttonImage = GetComponent<Image>();
        GetComponent<Button>().onClick.AddListener(ClickedOnMechanic);
    }

    private void OnEnable() {
        if (isBlocked) {
            buttonImage.color = blockedColor;
            GetComponentInChildren<TextMeshProUGUI>().color = blockedTextColor;
        }
        else if (mechMenu.mechanics.IsEnabled(mechanicName)) {
            isActive = true;
            buttonImage.color = activeColor;
        }
        else {
            isActive = false;
            buttonImage.color = inactiveColor;
        }
    }

    public void ClickedOnMechanic() {
        if (isBlocked) return;

        if (isActive) {
            // desativa
            DeactivateButtonImage();
            mechMenu.mechanics.Deactivate(mechanicName);
            mechMenu.changedMechanics = true;
            mechMenu.skillPoints++;
            mechMenu.UpdateSkillPointsText();
        }
        else {
            // ativa
            if (mechMenu.skillPoints == 0) return;
            // fazer text piscar, dar feedback de que ta sem pontos
            ActivateButtonImage();
            mechMenu.mechanics.Activate(mechanicName);
            mechMenu.changedMechanics = true;
            mechMenu.skillPoints--;
            mechMenu.UpdateSkillPointsText();
        }
    }

    public void UpdateTutorialInfo() {
        if (mechanicClip != null) {
            mechMenu.videoPlayer.clip = mechanicClip;
            mechMenu.videoPlayer.Play();
            mechMenu.rawImage.enabled = true;
        }
        mechMenu.title.text = mechanicName;
        mechMenu.description.text = description;
    }

    public void ClearTutorialInfo() {
        mechMenu.videoPlayer.Stop();
        mechMenu.rawImage.enabled = false;
        mechMenu.title.text = "";
        mechMenu.description.text = "";
    }

    public void ActivateButtonImage() {
        if (isBlocked) return;
        isActive = true;
        buttonImage.color = activeColor;
    }

    public void DeactivateButtonImage() {
        if (isBlocked) return;
        isActive = false;
        buttonImage.color = inactiveColor;
    }
}
