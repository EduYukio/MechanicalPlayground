using UnityEngine;

public class PlayerExplodingState : PlayerBaseState {
    private Vector3 rightDistance, leftDistance, upDistance, downDistance;
    private float xInput, yInput;

    public override void EnterState(PlayerFSM player) {
        Setup(player);
        PlayAnimation(player);
        PlayAudio();
        ShakeCamera(player);
        ExplosionAction(player);
    }

    public override void Update(PlayerFSM player) {
        if (CheckTransitionToWallSliding(player)) return;
        if (base.CheckTransitionToFalling(player)) return;
        if (base.CheckTransitionToGrounded(player)) return;
    }

    private void Setup(PlayerFSM player) {
        Helper.InputBuffer(out xInput, out yInput);
        player.explosionCooldownTimer = player.config.startExplosionCooldownTime;
        SetExplosionDistances(player);
    }

    private void PlayAnimation(PlayerFSM player) {
        player.animator.Play("PlayerExploding");
    }

    private void PlayAudio() {
        Manager.audio.Play("Explosion");
    }

    private void ShakeCamera(PlayerFSM player) {
        float shakeDuration = player.config.explosionShakeDuration;
        float shakeMagnitude = player.config.explosionShakeMagnitude;

        Manager.shaker.Shake(player.cameraObj, shakeDuration, shakeMagnitude);
    }

    private void ExplosionAction(PlayerFSM player) {
        Vector3 distance = CalculateExplosionDistance(player);
        Vector3 position = player.transform.position + distance;

        GameObject explosion = MonoBehaviour.Instantiate(player.explosionPrefab, position, Quaternion.identity);
        explosion.GetComponent<Animator>().Play("Explosion");

        LayerMask hitLayers = LayerMask.GetMask("Enemies", "Obstacles", "Projectiles", "Gate");
        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(position, player.config.explosionRadius, hitLayers);
        foreach (Collider2D targetHit in hitTargets) {
            CheckDamageEnemy(player, targetHit);
            CheckDestroyObject(player, targetHit);
        }
    }

    private void CheckDamageEnemy(PlayerFSM player, Collider2D targetHit) {
        bool hitEnemy = targetHit.gameObject.CompareTag("Enemy");

        if (hitEnemy) {
            Enemy enemy = targetHit.GetComponent<Enemy>();
            float damage = enemy.maxHealth * player.config.explosionDamageRate;
            enemy.TakeDamage(damage);
        }
    }

    private void CheckDestroyObject(PlayerFSM player, Collider2D targetHit) {
        bool hitObstacle = targetHit.gameObject.layer == LayerMask.NameToLayer("Obstacles");
        bool hitGate = targetHit.gameObject.layer == LayerMask.NameToLayer("Gate");
        bool hitProjectile = targetHit.gameObject.CompareTag("Projectile");

        if (hitObstacle || hitProjectile || hitGate) {
            MonoBehaviour.Destroy(targetHit.gameObject);
        }
    }

    private void SetExplosionDistances(PlayerFSM player) {
        float xRange = player.config.explosionXRange;
        float yRange = player.config.explosionYRange;

        rightDistance = new Vector3(xRange, -0.12f, 0f);
        leftDistance = new Vector3(-xRange, -0.12f, 0f);
        upDistance = new Vector3(0, yRange, 0f);
        downDistance = new Vector3(0, -yRange, 0f);
    }

    private Vector3 CalculateExplosionDistance(PlayerFSM player) {
        Vector3 inputDirection = base.GetFourDirectionalInput(player, xInput, yInput);

        if (inputDirection == Vector3.right) return rightDistance;
        if (inputDirection == Vector3.left) return leftDistance;
        if (inputDirection == Vector3.up) return upDistance;
        if (inputDirection == Vector3.down) return downDistance;

        return Vector3.zero;
    }

    public override bool CheckTransitionToWallSliding(PlayerFSM player) {
        if (!player.mechanics.IsEnabled("Wall Slide")) return false;

        if (player.isTouchingWall && !player.isGrounded) {
            player.TransitionToState(player.WallSlidingState);
            return true;
        }
        return false;
    }
}
