using UnityEngine;

public class PlayerAttackingState : PlayerBaseState {
    public LayerMask hitLayers;
    public float attackTimer;
    private Vector3 attackDirection;
    private bool isBoosted;
    private GameObject slashEffect;
    private bool shouldPogo = false;

    public override void EnterState(PlayerFSM player) {
        Manager.audio.Play("Slash1");
        Setup(player);
        PlayAttackAnimation(player);
        AttackAction(player);
    }

    private void PlayAttackAnimation(PlayerFSM player) {
        if (isBoosted) player.animator.Play("PlayerAttackingBoosted", -1, 0f);
        else player.animator.Play("PlayerAttacking", -1, 0f);
    }

    public override void Update(PlayerFSM player) {
        if (CheckTransitionToPogoing(player)) return;
        if (base.CheckTransitionToFalling(player)) return;
        if (base.CheckTransitionToGrounded(player)) return;
    }

    private void Setup(PlayerFSM player) {
        hitLayers = LayerMask.GetMask("Enemies", "Obstacles", "Projectiles", "BulletEthereal");
        player.attackCooldownTimer = player.config.startAttackCooldownTime;
        isBoosted = player.mechanics.IsEnabled("Range Boost");
        attackDirection = CalculateDirection(player);
        slashEffect = (isBoosted) ? player.boostedSlash : player.normalSlash;
        PositionSlashEffect(player, isBoosted);
    }

    private Vector3 CalculateDirection(PlayerFSM player) {
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");
        return base.GetFourDirectionalInput(player, xInput, yInput);
    }

    private void AttackAction(PlayerFSM player) {
        Vector2 hitboxSize;
        Vector2 attackPosition;
        if (isBoosted) {
            hitboxSize = new Vector2(3.65f, 3.65f) * GetBoostedSlashScale();
            attackPosition = player.transform.position + GetBoostedSlashPosition();
        }
        else {
            hitboxSize = new Vector2(3.65f, 3.65f) * GetNormalSlashScale();
            attackPosition = player.transform.position + GetNormalSlashPosition();
        }

        float attackRotationAngle = GetSlashAngleVector().z;
        Collider2D[] hitTargets = Physics2D.OverlapCapsuleAll(attackPosition, hitboxSize, CapsuleDirection2D.Horizontal, attackRotationAngle, hitLayers);
        foreach (Collider2D colliderHit in hitTargets) {
            CheckDamageEnemy(player, colliderHit);
            CheckPogo(player, colliderHit);
            CheckDestroyProjectile(player, colliderHit);
        }
    }

    private void CheckDamageEnemy(PlayerFSM player, Collider2D colliderHit) {
        bool hitEnemy = colliderHit.gameObject.CompareTag("Enemy");
        if (hitEnemy) {
            GameObject enemyObj = colliderHit.gameObject;
            enemyObj.GetComponent<Enemy>()?.TakeDamage(player.config.attackDamage);
        }
    }

    private void CheckDestroyProjectile(PlayerFSM player, Collider2D colliderHit) {
        if (!player.mechanics.IsEnabled("Destroy Projectile")) return;

        bool hitProjectile = colliderHit.gameObject.CompareTag("Projectile");
        if (hitProjectile) {
            Manager.audio.Play("Destroy Projectile");
            GameObject bulletObj = colliderHit.gameObject;
            EnemyBullet bullet = bulletObj.GetComponent<EnemyBullet>();
            MonoBehaviour.Instantiate(bullet.destroyBulletParticles, bullet.transform.position, Quaternion.identity);
            MonoBehaviour.Destroy(bulletObj);
        }
    }

    private void CheckPogo(PlayerFSM player, Collider2D colliderHit) {
        if (!player.mechanics.IsEnabled("Pogo Jump")) return;

        bool hitEnemy = colliderHit.gameObject.layer == LayerMask.NameToLayer("Enemies");
        bool hitProjectile = colliderHit.gameObject.layer == LayerMask.NameToLayer("Projectiles");
        bool hitObstacle = colliderHit.gameObject.layer == LayerMask.NameToLayer("Obstacles");

        bool downwardSlash = GetSlashAngleVector().z == 270f;
        shouldPogo = downwardSlash && !player.isGrounded && (hitEnemy || hitProjectile || hitObstacle);
    }

    private Vector3 GetBoostedSlashPosition() {
        if (attackDirection == Vector3.right) return new Vector3(1, -0.16f, 0f);
        if (attackDirection == Vector3.up) return new Vector3(0, 0.95f, 0f);
        if (attackDirection == Vector3.left) return new Vector3(-1, -0.16f, 0f);
        if (attackDirection == Vector3.down) return new Vector3(0, -1.23f, 0f);

        return Vector3.zero;
    }

    private Vector3 GetNormalSlashPosition() {
        if (attackDirection == Vector3.right) return new Vector3(0.7f, -0.15f, 0f);
        if (attackDirection == Vector3.up) return new Vector3(0, 0.59f, 0);
        if (attackDirection == Vector3.left) return new Vector3(-0.7f, -0.15f, 0f);
        if (attackDirection == Vector3.down) return new Vector3(0, -0.86f, 0f);

        return Vector3.zero;
    }

    private Vector3 GetSlashAngleVector() {
        if (attackDirection == Vector3.right) return new Vector3(0, 0f, 0f);
        if (attackDirection == Vector3.up) return new Vector3(0, 0f, 90f);
        if (attackDirection == Vector3.left) return new Vector3(0, 0f, 180f);
        if (attackDirection == Vector3.down) return new Vector3(0, 0f, 270f);

        return Vector3.zero;
    }

    private Vector2 GetBoostedSlashScale() {
        return new Vector2(0.64f, 0.5f);
    }

    private Vector2 GetNormalSlashScale() {
        return new Vector2(0.32f, 0.25f);
    }

    private void PositionSlashEffect(PlayerFSM player, bool isBoosted) {
        if (isBoosted) {
            slashEffect.transform.localPosition = GetBoostedSlashPosition();
        }
        else {
            slashEffect.transform.localPosition = GetNormalSlashPosition();
        }
        slashEffect.transform.eulerAngles = GetSlashAngleVector();
    }

    public bool CheckTransitionToPogoing(PlayerFSM player) {
        if (!player.mechanics.IsEnabled("Pogo Jump")) return false;

        if (shouldPogo) {
            shouldPogo = false;
            player.TransitionToState(player.PogoingState);
            return true;
        }

        return false;
    }
}
