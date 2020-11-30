using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayerOnTouch : MonoBehaviour {
    bool isSpike = false;

    private void Start() {
        isSpike = gameObject.name.Contains("Spike");
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            if (isSpike && PlayerInvulnerableToSpike(other.gameObject)) return;
            KillPlayer(other.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        KillPlayer(other.gameObject);
    }

    void KillPlayer(GameObject otherObject) {
        if (otherObject.CompareTag("Player")) {
            PlayerFSM player = otherObject.GetComponent<PlayerFSM>();
            player.TransitionToState(player.DyingState);
        }
    }

    bool PlayerInvulnerableToSpike(GameObject otherObject) {
        PlayerFSM player = otherObject.GetComponent<PlayerFSM>();
        if (player.mechanics.IsEnabled("SpikeInvulnerability")) return true;
        return false;
    }
}
