using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour {
    void Start() {

    }

    void Update() {

    }

    private void OnCollisionEnter2D(Collision2D other) {
        // check if player has protection
        if (other.gameObject.CompareTag("Player")) {
            PlayerFSM player = other.gameObject.GetComponent<PlayerFSM>();
            player.Die();
        }
    }
}
