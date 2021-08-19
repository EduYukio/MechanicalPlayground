using UnityEngine;

public class GunBullet : MonoBehaviour {
    private static PlayerFSM player;
    private static string[] obstacleTags = { "Ground", "Obstacle", "Gate" };

    private void Start() {
        if (player == null) player = GameObject.FindObjectOfType<PlayerFSM>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        ProcessCollision(other.gameObject);
    }

    private void ProcessCollision(GameObject collidedObj) {
        if (collidedObj.CompareTag("Enemy")) {
            collidedObj.GetComponent<Enemy>()?.TakeDamage(player.config.gunBootsDamage);
            Destroy(gameObject);
            return;
        }

        foreach (var tag in obstacleTags) {
            if (collidedObj.CompareTag(tag)) Destroy(gameObject);
            return;
        }
    }
}