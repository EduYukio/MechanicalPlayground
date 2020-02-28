using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public GameObject menu;

    void Start() {
    }

    void Update() {
        if (Input.GetButtonDown("Cancel")) {
            OpenOrCloseMenu();
        }
    }

    public void OpenOrCloseMenu() {
        menu.SetActive(!menu.activeSelf);
        if (menu.activeSelf) {
            Time.timeScale = 0;
        }
        else {
            Time.timeScale = 1;
        }
    }
}
