using System;
using UnityEngine;

public class Spike : MonoBehaviour {
    private void OnCollisionEnter2D(Collision2D other) {
        ProcessCollision(other.gameObject);
    }

    private void OnCollisionStay2D(Collision2D other) {
        ProcessCollision(other.gameObject);
    }

    private void ProcessCollision(GameObject collidedObj) {
        if (!collidedObj.CompareTag("Player")) return;
        if (PlayerIsInvulnerableToSpike(collidedObj)) return;

        KillPlayer(collidedObj);
    }

    bool PlayerIsInvulnerableToSpike(GameObject collidedObj) {
        PlayerFSM player = collidedObj.GetComponent<PlayerFSM>();
        if (player.mechanics.IsEnabled("Spike Invulnerability")) return true;

        return false;
    }

    void KillPlayer(GameObject collidedObj) {
        PlayerFSM player = collidedObj.GetComponent<PlayerFSM>();
        player.TransitionToState(player.DyingState);
    }
}