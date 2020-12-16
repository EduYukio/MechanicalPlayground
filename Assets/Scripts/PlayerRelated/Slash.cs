using UnityEngine;

public class Slash : MonoBehaviour {
    PlayerFSM player;

    private void Start() {
        player = transform.parent.GetComponent<PlayerFSM>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        bool hitEnemy = other.gameObject.CompareTag("Enemy");
        bool hitProjectile = other.gameObject.CompareTag("Projectile");
        // bool hitObstacle = other.gameObject.CompareTag("Obstacle");

        if (hitEnemy) {
            other.GetComponent<Enemy>()?.TakeDamage(player.config.attackDamage);
        }

        if (hitProjectile && player.mechanics.IsEnabled("Destroy Projectile")) {
            Destroy(other.gameObject);
        }
    }
}