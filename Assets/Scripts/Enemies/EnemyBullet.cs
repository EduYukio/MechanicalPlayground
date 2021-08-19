using UnityEngine;

public class EnemyBullet : MonoBehaviour {
    private PlayerFSM player;

    public Rigidbody2D rb;
    public bool alreadyProcessedHit = false;
    public float initialSpeed;
    public Vector3 initialDirection;
    public float hitsToKill;

    public GameObject parryParticles;
    public GameObject destroyBulletParticles;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        if (initialSpeed != 0f) {
            rb.velocity = initialDirection * initialSpeed;
        }

        player = GameObject.FindObjectOfType<PlayerFSM>();
        hitsToKill = player.config.reflectedBulletsNeededToKill;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        ProcessCollision(other.gameObject);
    }

    private void ProcessCollision(GameObject collidedObj) {
        if (alreadyProcessedHit) return;

        if (HitPlayerWithShield(collidedObj)) return;
        if (HitShield(collidedObj)) return;

        if (collidedObj.CompareTag("Enemy")) {
            DamageEnemy(collidedObj);
            return;
        }

        string[] obstacleTags = { "Player", "Ground", "Obstacle", "Gate" };
        foreach (var tag in obstacleTags) {
            if (collidedObj.CompareTag(tag)) {
                Instantiate(destroyBulletParticles, transform.position, Quaternion.identity);
                Destroy(gameObject);
                if (tag == "Player") KillPlayer();
                return;
            }
        }
    }

    private bool HitPlayerWithShield(GameObject collidedObj) {
        if (!collidedObj.CompareTag("Player")) return false;

        if (player.shield.gameObject.activeSelf) {
            BulletHitShieldAction(player);
            return true;
        }

        return false;
    }

    private bool HitShield(GameObject collidedObj) {
        if (!collidedObj.CompareTag("Shield")) return false;

        BulletHitShieldAction(player);
        return true;
    }

    private void BulletHitShieldAction(PlayerFSM player) {
        alreadyProcessedHit = true;

        if (player.parryTimer > 0) {
            Instantiate(parryParticles, transform.position, Quaternion.identity);
            player.shield.Parry(gameObject);
        }
        else {
            Instantiate(destroyBulletParticles, transform.position, Quaternion.identity);
            Destroy(gameObject);
            player.shield.ConsumeShield();
        }
    }

    private void DamageEnemy(GameObject collidedObj) {
        Enemy enemy = collidedObj.GetComponent<Enemy>();
        float damage = 0.1f + enemy.maxHealth / hitsToKill;
        enemy.TakeDamage(damage);
        Destroy(gameObject);
    }

    private void KillPlayer() {
        player.TransitionToState(player.DyingState);
    }

    public void ReflectBullet() {
        alreadyProcessedHit = false;
        rb.velocity = -4 * rb.velocity;
        Vector3 angle = transform.eulerAngles;
        transform.eulerAngles = new Vector3(angle.x, angle.y, angle.z + 180);
        if (name.Contains("Ethereal")) {
            gameObject.layer = LayerMask.NameToLayer("EtherealParriedBullet");
        }
        else {
            gameObject.layer = LayerMask.NameToLayer("ParriedBullet");
        }
    }
}