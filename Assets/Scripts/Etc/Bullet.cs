using UnityEngine;

public class Bullet : MonoBehaviour {
    public Rigidbody2D rb;
    public bool alreadyProcessedHit = false;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        ProcessCollision(other.gameObject);
    }

    void ProcessCollision(GameObject collidedObj) {
        if (alreadyProcessedHit) return;

        if (HitPlayerWithShield(collidedObj) || HitShield(collidedObj)) {
            return;
        }

        bool hitEnemy = collidedObj.CompareTag("Enemy");
        if (hitEnemy) {
            DamageEnemy(collidedObj);
            return;
        }

        bool hitPlayer = collidedObj.CompareTag("Player");
        bool hitGround = collidedObj.CompareTag("Ground");
        bool hitObstacle = collidedObj.CompareTag("Obstacle");
        bool shouldAutoDestroy = hitPlayer || hitGround || hitObstacle;

        if (shouldAutoDestroy) Destroy(gameObject);
        if (hitPlayer) KillPlayer(collidedObj);
    }

    bool HitPlayerWithShield(GameObject collidedObj) {
        if (!collidedObj.CompareTag("Player")) return false;

        PlayerFSM player = collidedObj.GetComponent<PlayerFSM>();
        if (player.shield.gameObject.activeSelf) {
            BulletHitShieldAction(player);
            return true;
        }
        return false;
    }


    bool HitShield(GameObject collidedObj) {
        if (!collidedObj.CompareTag("Shield")) return false;

        PlayerFSM player = collidedObj.transform.parent.GetComponent<PlayerFSM>();
        BulletHitShieldAction(player);
        return true;
    }

    void BulletHitShieldAction(PlayerFSM player) {
        alreadyProcessedHit = true;

        if (player.parryTimer > 0) {
            player.shield.Parry(gameObject);
        }
        else {
            Destroy(gameObject);
            player.shield.ConsumeShield();
        }
    }

    void DamageEnemy(GameObject collidedObj) {
        Enemy enemy = collidedObj.GetComponent<Enemy>();
        float damage = 0.1f + enemy.maxHealth / 2;
        enemy.TakeDamage(damage);
        Destroy(gameObject);
    }

    void KillPlayer(GameObject collidedObj) {
        PlayerFSM player = collidedObj.GetComponent<PlayerFSM>();
        player.TransitionToState(player.DyingState);
    }
}