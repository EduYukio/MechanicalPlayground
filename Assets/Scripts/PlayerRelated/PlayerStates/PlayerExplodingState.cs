using UnityEngine;

public class PlayerExplodingState : PlayerBaseState {
    public LayerMask hitLayers;
    float xInput, yInput;
    Vector3 rightDistance, leftDistance, upDistance, downDistance;

    public override void EnterState(PlayerFSM player) {
        player.animator.Play("PlayerExploding");
        Manager.audio.Play("Explosion");
        Setup(player);
        ExplosionAction(player);
    }

    public override void Update(PlayerFSM player) {
        if (CheckTransitionToWallSliding(player)) return;
        if (base.CheckTransitionToFalling(player)) return;
        if (base.CheckTransitionToGrounded(player)) return;
    }

    void Setup(PlayerFSM player) {
        InputBuffer();
        SetExplosionRanges(player);
        hitLayers = LayerMask.GetMask("Enemies", "Obstacles", "Projectiles", "Gate");
        player.explosionCooldownTimer = player.config.startExplosionCooldownTime;
    }

    void ExplosionAction(PlayerFSM player) {
        Vector3 explosionDistance = CalculateExplosionDistance(player);
        Vector3 explosionPosition = player.transform.position + explosionDistance;
        GameObject explosion = MonoBehaviour.Instantiate(player.explosionPrefab, explosionPosition, Quaternion.identity);
        explosion.GetComponent<Animator>().Play("Explosion");

        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(explosionPosition, player.config.explosionRadius, hitLayers);
        foreach (Collider2D colliderHit in hitTargets) {
            CheckDamageEnemy(player, colliderHit);
            CheckDestroyObject(player, colliderHit);
        }
    }

    void CheckDamageEnemy(PlayerFSM player, Collider2D colliderHit) {
        bool hitEnemy = colliderHit.gameObject.CompareTag("Enemy");
        if (hitEnemy) {
            Enemy enemy = colliderHit.GetComponent<Enemy>();
            float damage = enemy.maxHealth * player.config.explosionDamageRate;
            enemy.TakeDamage(damage);
        }
    }

    void CheckDestroyObject(PlayerFSM player, Collider2D colliderHit) {
        bool hitObstacle = colliderHit.gameObject.layer == LayerMask.NameToLayer("Obstacles");
        bool hitGate = colliderHit.gameObject.layer == LayerMask.NameToLayer("Gate");
        bool hitProjectile = colliderHit.gameObject.CompareTag("Projectile");
        if (hitObstacle || hitProjectile || hitGate) {
            MonoBehaviour.Destroy(colliderHit.gameObject);
        }
    }

    void InputBuffer() {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
    }

    public override bool CheckTransitionToWallSliding(PlayerFSM player) {
        if (!player.mechanics.IsEnabled("Wall Slide")) return false;

        if (player.isTouchingWall && !player.isGrounded) {
            player.TransitionToState(player.WallSlidingState);
            return true;
        }
        return false;
    }

    void SetExplosionRanges(PlayerFSM player) {
        float xRange = player.config.xRange;
        float yRange = player.config.yRange;

        rightDistance = new Vector3(xRange, -0.12f, 0f);
        leftDistance = new Vector3(-xRange, -0.12f, 0f);
        upDistance = new Vector3(0, yRange, 0f);
        downDistance = new Vector3(0, -yRange, 0f);
    }

    Vector3 CalculateExplosionDistance(PlayerFSM player) {
        Vector3 inputDirection = base.GetFourDirectionalInput(player, xInput, yInput);

        if (inputDirection == Vector3.right) return rightDistance;
        if (inputDirection == Vector3.left) return leftDistance;
        if (inputDirection == Vector3.up) return upDistance;
        if (inputDirection == Vector3.down) return downDistance;

        return Vector3.zero;
    }
}
