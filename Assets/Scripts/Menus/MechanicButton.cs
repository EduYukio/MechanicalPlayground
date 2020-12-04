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
    public static MechanicsMenu menu;

    public bool isUnavailable = false;
    public bool isActive = false;

    private Image buttonImage;
    private Color activeColor = new Color(100 / 255f, 122 / 255f, 224 / 255f, 1f);
    private Color inactiveColor = Color.gray;
    private Color unavailableColor = new Color(0.1f, 0.1f, 0.1f, 1f);

    private void Awake() {
        if (menu == null) menu = GameObject.FindObjectOfType<MechanicsMenu>();
        buttonImage = GetComponent<Image>();
        GetComponent<Button>().onClick.AddListener(ClickedOnMechanic);
    }

    private void OnEnable() {
        if (isUnavailable) {
            buttonImage.color = unavailableColor;
        }
        else if (menu.mechanics.IsEnabled(mechanicName)) {
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
            menu.mechanics.Deactivate(mechanicName);
        }
        else {
            // ativa
            ActivateButtonImage();
            menu.mechanics.Activate(mechanicName);
        }
    }

    public void UpdateTutorialInfo() {
        if (mechanicClip != null) {
            menu.videoPlayer.clip = mechanicClip;
            menu.videoPlayer.Play();
            menu.rawImage.enabled = true;
        }
        menu.title.text = mechanicName;
        menu.description.text = description;
    }

    public void ClearTutorialInfo() {
        menu.videoPlayer.Stop();
        menu.rawImage.enabled = false;
        menu.title.text = "";
        menu.description.text = "";
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
