using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OkButton : MonoBehaviour {
    public Slots slots;
    public GameObject player;
    public GameObject menu;

    void Start() {
        slots = FindObjectOfType<Slots>();
        player = GameObject.Find("Player");
        menu = GameObject.Find("MechanicsMenu");
    }

    public void OkAction() {
        if (slots.lastIndexAvailable < 3) {
            return;
        }

        foreach (string mechanic in slots.mechanics) {
            System.Type mType = System.Type.GetType(mechanic);
            player.AddComponent(mType);
        }

        menu.SetActive(false);
        Time.timeScale = 1;
    }
}
