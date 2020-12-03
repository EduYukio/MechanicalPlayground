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

    void Start() {
        if (menu == null) menu = GameObject.FindObjectOfType<MechanicsMenu>();

        GetComponent<Button>().onClick.AddListener(ClickedOnMechanic);
    }

    void Update() {

    }

    public void ClickedOnMechanic() {
        if (isUnavailable) return;

        if (isActive) {
            // desativa
            menu.mechanics.Deactivate(mechanicName);
            isActive = false;
        }
        else {
            // ativa
            menu.mechanics.Activate(mechanicName);
            isActive = true;
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
}
