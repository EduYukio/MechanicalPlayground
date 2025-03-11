using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private bool shouldPogo;
    private bool isRangeBoosted;
    private Vector3 attackDirection;
    private GameObject slashEffect;
    private float playerDamage;

    public override void EnterState(PlayerFSM player)
    {
        Setup(player);
        PlayAnimation(player);
        PlayAudio();
        AttackAction(player);
    }

    public override void Update(PlayerFSM player)
    {
        if (CheckTransitionToPogoing(player)) return;
        if (base.CheckTransitionToFalling(player)) return;
        if (base.CheckTransitionToGrounded(player)) return;
    }

    private void Setup(PlayerFSM player)
    {
        shouldPogo = false;
        player.attackCooldownTimer = player.config.startAttackCooldownTime;
        isRangeBoosted = player.mechanics.IsEnabled("Range Boost");
        attackDirection = CalculateDirection(player);
        slashEffect = (isRangeBoosted) ? player.boostedSlash : player.normalSlash;
        playerDamage = player.config.attackDamage;
        PositionSlashEffect(player, isRangeBoosted);
    }

    private void PlayAnimation(PlayerFSM player)
    {
        if (isRangeBoosted) player.animator.Play("PlayerAttackingBoosted", -1, 0f);
        else player.animator.Play("PlayerAttacking", -1, 0f);
    }

    private void PlayAudio()
    {
        Manager.audio.Play("Slash1");
    }

    private void AttackAction(PlayerFSM player)
    {
        Vector2 hitboxSize = CalculateHitBoxSize();
        Vector3 attackPosition = CalculateAttackPosition(player);
        LayerMask hitLayers = LayerMask.GetMask("Enemies", "Obstacles", "Projectiles", "BulletEthereal");

        float attackRotationAngle = GetSlashAngleVector().z;
        Collider2D[] hitTargets = Physics2D.OverlapCapsuleAll(attackPosition, hitboxSize, CapsuleDirection2D.Horizontal, attackRotationAngle, hitLayers);
        foreach (Collider2D targetHit in hitTargets)
        {
            CheckDamageEnemy(player, targetHit);
            CheckPogo(player, targetHit);
            CheckDestroyProjectile(player, targetHit);
        }
    }

    private Vector3 CalculateDirection(PlayerFSM player)
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");
        return base.GetFourDirectionalInput(player, xInput, yInput);
    }

    private void PositionSlashEffect(PlayerFSM player, bool isBoosted)
    {
        if (isBoosted)
        {
            slashEffect.transform.localPosition = GetBoostedSlashPosition();
        }
        else
        {
            slashEffect.transform.localPosition = GetNormalSlashPosition();
        }
        slashEffect.transform.eulerAngles = GetSlashAngleVector();
    }

    private Vector2 CalculateHitBoxSize()
    {
        Vector2 baseHitboxSize = new Vector2(3.65f, 3.65f);

        if (isRangeBoosted) return baseHitboxSize * GetBoostedSlashScale();
        return baseHitboxSize * GetNormalSlashScale();
    }

    private Vector2 CalculateAttackPosition(PlayerFSM player)
    {
        Vector3 baseAttackPosition = player.transform.position;

        if (isRangeBoosted) return baseAttackPosition + GetBoostedSlashPosition();
        return baseAttackPosition + GetNormalSlashPosition();
    }

    private void CheckDamageEnemy(PlayerFSM player, Collider2D targetHit)
    {
        bool hitEnemy = targetHit.gameObject.CompareTag("Enemy");

        if (hitEnemy)
        {
            GameObject enemyObj = targetHit.gameObject;
            enemyObj.GetComponent<Enemy>()?.TakeDamage(playerDamage);
        }
    }

    private void CheckPogo(PlayerFSM player, Collider2D targetHit)
    {
        if (!player.mechanics.IsEnabled("Pogo Jump")) return;

        bool hitEnemy = targetHit.gameObject.layer == LayerMask.NameToLayer("Enemies");
        bool hitProjectile = targetHit.gameObject.layer == LayerMask.NameToLayer("Projectiles");
        bool hitObstacle = targetHit.gameObject.layer == LayerMask.NameToLayer("Obstacles");

        bool downwardSlash = GetSlashAngleVector().z == 270f;
        shouldPogo = !player.isGrounded && downwardSlash && (hitEnemy || hitProjectile || hitObstacle);
    }

    private void CheckDestroyProjectile(PlayerFSM player, Collider2D targetHit)
    {
        if (!player.mechanics.IsEnabled("Destroy Projectile")) return;

        bool hitProjectile = targetHit.gameObject.CompareTag("Projectile");
        if (hitProjectile)
        {
            Manager.audio.Play("Destroy Projectile");
            GameObject bulletObj = targetHit.gameObject;
            EnemyBullet bullet = bulletObj.GetComponent<EnemyBullet>();
            MonoBehaviour.Instantiate(bullet.destroyBulletParticles, bullet.transform.position, Quaternion.identity);
            MonoBehaviour.Destroy(bulletObj);
        }
    }

    private Vector3 GetBoostedSlashPosition()
    {
        if (attackDirection == Vector3.right) return new Vector3(1, -0.16f, 0f);
        if (attackDirection == Vector3.up) return new Vector3(0, 0.95f, 0f);
        if (attackDirection == Vector3.left) return new Vector3(-1, -0.16f, 0f);
        if (attackDirection == Vector3.down) return new Vector3(0, -1.23f, 0f);

        return Vector3.zero;
    }

    private Vector3 GetNormalSlashPosition()
    {
        if (attackDirection == Vector3.right) return new Vector3(0.7f, -0.15f, 0f);
        if (attackDirection == Vector3.up) return new Vector3(0, 0.59f, 0);
        if (attackDirection == Vector3.left) return new Vector3(-0.7f, -0.15f, 0f);
        if (attackDirection == Vector3.down) return new Vector3(0, -0.86f, 0f);

        return Vector3.zero;
    }

    private Vector3 GetSlashAngleVector()
    {
        if (attackDirection == Vector3.right) return new Vector3(0, 0f, 0f);
        if (attackDirection == Vector3.up) return new Vector3(0, 0f, 90f);
        if (attackDirection == Vector3.left) return new Vector3(0, 0f, 180f);
        if (attackDirection == Vector3.down) return new Vector3(0, 0f, 270f);

        return Vector3.zero;
    }

    private Vector2 GetBoostedSlashScale()
    {
        return new Vector2(0.64f, 0.5f);
    }

    private Vector2 GetNormalSlashScale()
    {
        return new Vector2(0.32f, 0.25f);
    }

    public bool CheckTransitionToPogoing(PlayerFSM player)
    {
        if (!player.mechanics.IsEnabled("Pogo Jump")) return false;

        if (shouldPogo)
        {
            shouldPogo = false;
            player.TransitionToState(player.PogoingState);
            return true;
        }

        return false;
    }
}
