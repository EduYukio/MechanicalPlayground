using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OkButton : MonoBehaviour {
    public Slots slots;
    public GameObject player;

    void Start() {
        slots = FindObjectOfType<Slots>();
        player = GameObject.Find("Player");
    }

    public void OkAction() {
        if (slots.mechanics.Length < 3) {
            return;
        }

        foreach (string mechanic in slots.mechanics) {
            System.Type mType = System.Type.GetType(mechanic);
            player.AddComponent(mType);
        }

        //disable menu
    }
}
