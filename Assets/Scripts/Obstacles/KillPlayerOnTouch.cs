using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayerOnTouch : MonoBehaviour {
    bool isSpike = false;
    bool isSaw = false;
    bool isBullet = false;

    private void Start() {
        isSpike = gameObject.name.Contains("Spike");
        isSaw = gameObject.name.Contains("Saw");
        isBullet = gameObject.name.Contains("Bullet");
    }

    private void OnCollisionEnter2D(Collision2D other) {
        ProcessCollision(other.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        ProcessCollision(other.gameObject);
    }

    void ProcessCollision(GameObject collidedObj) {
        if (isBullet) {
            if (CheckBulletHitObstacle(collidedObj)) return;
            if (CheckBulletHitGround(collidedObj)) return;
            if (CheckBulletHitPlayer(collidedObj)) return;
        }
        else if (isSpike) {
            if (PlayerIsInvulnerableToSpike(collidedObj)) return;

            KillPlayer(collidedObj);
        }
        else if (isSaw) {
            if (PlayerIsInvulnerableToSaw(collidedObj)) return;

            KillPlayer(collidedObj);
        }
        else {
            if (CheckHitPlayer(collidedObj)) return;
        }
    }

    void KillPlayer(GameObject collidedObj) {
        PlayerFSM player = collidedObj.GetComponent<PlayerFSM>();
        player.TransitionToState(player.DyingState);
    }

    bool PlayerIsInvulnerableToSpike(GameObject collidedObj) {
        PlayerFSM player = collidedObj.GetComponent<PlayerFSM>();
        if (player.mechanics.IsEnabled("Spike Invulnerability")) return true;

        return false;
    }

    bool PlayerIsInvulnerableToSaw(GameObject collidedObj) {
        PlayerFSM player = collidedObj.GetComponent<PlayerFSM>();
        if (player.mechanics.IsEnabled("Saw Invulnerability")) return true;

        return false;
    }

    bool CheckBulletHitPlayer(GameObject collidedObj) {
        if (collidedObj.CompareTag("Player")) {
            KillPlayer(collidedObj);
            Destroy(gameObject);
            return true;
        }
        return false;
    }

    bool CheckBulletHitGround(GameObject collidedObj) {
        if (collidedObj.CompareTag("Ground")) {
            Destroy(gameObject);
            return true;
        }
        return false;
    }

    bool CheckBulletHitObstacle(GameObject collidedObj) {
        if (collidedObj.CompareTag("Obstacle")) {
            Destroy(gameObject);
            return true;
        }
        return false;
    }

    bool CheckHitPlayer(GameObject collidedObj) {
        if (collidedObj.CompareTag("Player")) {
            KillPlayer(collidedObj);
            return true;
        }
        return false;
    }
}
