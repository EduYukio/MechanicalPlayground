using UnityEngine;

public class GunBullet : MonoBehaviour {
    static PlayerFSM player;

    private void Start() {
        GunBullet.player = GameObject.Find("PlayerFSM").GetComponent<PlayerFSM>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        ProcessCollision(other.gameObject);
    }

    void ProcessCollision(GameObject collidedObj) {
        bool hitEnemy = collidedObj.CompareTag("Enemy");
        if (hitEnemy) {
            collidedObj.GetComponent<Enemy>()?.TakeDamage(player.config.gunBootsDamage);
        }

        bool hitGround = collidedObj.CompareTag("Ground");
        bool hitObstacle = collidedObj.CompareTag("Obstacle");
        bool shouldAutoDestroy = hitEnemy || hitGround || hitObstacle;

        if (shouldAutoDestroy) Destroy(gameObject);
    }
}