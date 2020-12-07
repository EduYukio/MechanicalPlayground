using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayerOnTouch : MonoBehaviour {
    bool isSpike = false;
    bool isBullet = false;

    private void Start() {
        isSpike = gameObject.name.Contains("Spike");
        isBullet = gameObject.name.Contains("Bullet");
    }

    // spikes, enemy body
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            if (isSpike && PlayerInvulnerableToSpike(other.gameObject)) return;
            KillPlayer(other.gameObject);
        }
    }

    // saw, bullets
    private void OnTriggerEnter2D(Collider2D other) {
        if (isBullet) {
            if (CheckBulletHitPlayer(other.gameObject)) return;
            if (CheckBulletHitGround(other.gameObject)) return;
        }
        else {
            KillPlayer(other.gameObject);
        }
    }

    void KillPlayer(GameObject otherObject) {
        PlayerFSM player = otherObject.GetComponent<PlayerFSM>();
        player.TransitionToState(player.DyingState);
    }

    bool PlayerInvulnerableToSpike(GameObject otherObject) {
        PlayerFSM player = otherObject.GetComponent<PlayerFSM>();
        if (player.mechanics.IsEnabled("Spike Invulnerability")) return true;

        return false;
    }

    bool CheckBulletHitPlayer(GameObject otherObject) {
        if (otherObject.CompareTag("Player")) {
            KillPlayer(otherObject);
            Destroy(gameObject);
            return true;
        }
        return false;
    }
    bool CheckBulletHitGround(GameObject otherObject) {
        if (otherObject.CompareTag("Ground")) {
            Destroy(gameObject);
            return true;
        }
        return false;
    }
}
