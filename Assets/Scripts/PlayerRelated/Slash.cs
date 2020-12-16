using System;
using UnityEngine;

public class Slash : MonoBehaviour {
    PlayerFSM player;
    // bool player.shouldPogo;
    private void Start() {
        player = transform.parent.GetComponent<PlayerFSM>();
    }

    private void Update() {
        if (player.shouldPogo) {
            Pogo();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        bool hitEnemy = other.gameObject.CompareTag("Enemy");
        bool hitProjectile = other.gameObject.CompareTag("Projectile");
        bool hitObstacle = other.gameObject.CompareTag("Obstacle");

        if (hitEnemy) {
            other.GetComponent<Enemy>()?.TakeDamage(player.config.attackDamage);
        }

        if (hitProjectile && player.mechanics.IsEnabled("Destroy Projectile")) {
            Destroy(other.gameObject);
        }

        bool downwardSlash = transform.eulerAngles.z == 270f;
        player.shouldPogo = downwardSlash && !player.isGrounded && (hitEnemy || hitProjectile || hitObstacle);
    }

    private void Pogo() {
        player.shouldPogo = false;
        // player.rb.velocity = Vector3.zero;
        player.TransitionToState(player.PogoingState);
    }
}