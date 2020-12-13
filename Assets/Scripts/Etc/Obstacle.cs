using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {
    bool isSpike = false;
    bool isSaw = false;
    bool isBullet = false;
    bool alreadyInverted = false;

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

    private void OnTriggerStay2D(Collider2D other) {
        ProcessCollision(other.gameObject);
    }

    private void OnCollisionStay2D(Collision2D other) {
        ProcessCollision(other.gameObject);
    }

    void ProcessCollision(GameObject collidedObj) {
        if (isBullet) {
            ProcessBulletHit(collidedObj);
        }
        else if (isSpike) {
            if (!collidedObj.CompareTag("Player")) return;
            if (PlayerIsInvulnerableToSpike(collidedObj)) return;

            KillPlayer(collidedObj);
        }
        else if (isSaw) {
            if (!collidedObj.CompareTag("Player")) return;
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

    void ProcessBulletHit(GameObject collidedObj) {
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

    bool CheckHitPlayer(GameObject collidedObj) {
        if (collidedObj.CompareTag("Player")) {
            KillPlayer(collidedObj);
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

    bool HitPlayerWithShield(GameObject collidedObj) {
        if (!collidedObj.CompareTag("Player")) return false;

        PlayerFSM player = collidedObj.GetComponent<PlayerFSM>();
        if (player.shield.gameObject.activeSelf) {
            BulletHitShieldAction(player);
            return true;
        }
        return false;
    }

    void BulletHitShieldAction(PlayerFSM player) {
        if (player.parryTimer > 0) {
            player.shield.Parry();
            InvertDirection(gameObject);
        }
        else {
            Destroy(gameObject);
            player.shield.ConsumeShield();
        }
    }

    private void InvertDirection(GameObject bullet) {
        if (alreadyInverted) return;

        alreadyInverted = true;
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = -2 * rb.velocity;
        Vector3 angle = bullet.transform.eulerAngles;
        bullet.transform.eulerAngles = new Vector3(angle.x, angle.y, angle.z + 180);
        bullet.layer = LayerMask.NameToLayer("ParriedBullet");
    }

    void DamageEnemy(GameObject collidedObj) {
        Enemy enemy = collidedObj.GetComponent<Enemy>();
        float damage = 0.1f + enemy.maxHealth / 2;
        enemy.TakeDamage(damage);
        Destroy(gameObject);
    }
}
