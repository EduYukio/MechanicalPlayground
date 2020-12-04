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

    public bool isUnavailable = false;
    public bool isActive = false;

    private Image buttonImage;
    private Color activeColor = new Color(100 / 255f, 122 / 255f, 224 / 255f, 1f);
    private Color inactiveColor = Color.gray;
    private Color unavailableColor = new Color(0.1f, 0.1f, 0.1f, 1f);

    private void Awake() {
        if (mechMenu == null) mechMenu = GameObject.FindObjectOfType<MechanicsMenu>();
        buttonImage = GetComponent<Image>();
        GetComponent<Button>().onClick.AddListener(ClickedOnMechanic);
    }

    private void OnEnable() {
        if (isUnavailable) {
            buttonImage.color = unavailableColor;
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
        if (isUnavailable) return;

        if (isActive) {
            // desativa
            DeactivateButtonImage();
            mechMenu.mechanics.Deactivate(mechanicName);
            mechMenu.changedMechanics = true;
        }
        else {
            // ativa
            ActivateButtonImage();
            mechMenu.mechanics.Activate(mechanicName);
            mechMenu.changedMechanics = true;
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
        if (isUnavailable) return;
        isActive = true;
        buttonImage.color = activeColor;
    }

    public void DeactivateButtonImage() {
        if (isUnavailable) return;
        isActive = false;
        buttonImage.color = inactiveColor;
    }
}
