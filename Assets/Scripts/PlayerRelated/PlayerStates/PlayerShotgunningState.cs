using UnityEngine;

public class PlayerShotgunningState : PlayerBaseState {
    public LayerMask hitLayers;
    float xInput;
    float yInput;
    float explosionRadius = 0.4f;
    float explosionDamageRate = 0.5f;
    Vector3 rightDistance = new Vector3(1.5f, -0.12f, 0f);
    Vector3 leftDistance = new Vector3(-1.5f, -0.12f, 0f);
    Vector3 upDistance = new Vector3(0, 1.6f, 0f);
    Vector3 downDistance = new Vector3(0, -1.6f, 0f);

    public override void EnterState(PlayerFSM player) {
        player.animator.Play("PlayerShotgunning");
        Setup(player);
        ShotgunAction(player);
    }

    public override void Update(PlayerFSM player) {
        base.ProcessMovementInput(player);

        if (base.CheckTransitionToFalling(player)) return;
        if (base.CheckTransitionToGrounded(player)) return;
        if (base.CheckTransitionToWalking(player)) return;
    }

    void Setup(PlayerFSM player) {
        InputBuffer();
        hitLayers = LayerMask.GetMask("Enemies", "Obstacles", "Projectiles");
    }

    void ShotgunAction(PlayerFSM player) {
        Vector3 inputDirection = base.GetFourDirectionalInput(player, xInput, yInput);
        Vector3 explosionDistance = new Vector3();
        if (inputDirection == Vector3.right) explosionDistance = rightDistance;
        else if (inputDirection == Vector3.left) explosionDistance = leftDistance;
        else if (inputDirection == Vector3.up) explosionDistance = upDistance;
        else if (inputDirection == Vector3.down) explosionDistance = downDistance;

        Vector3 explosionPosition = player.transform.position + explosionDistance;
        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(explosionPosition, explosionRadius, hitLayers);

        foreach (Collider2D colliderHit in hitTargets) {
            CheckDamageEnemy(player, colliderHit);
            CheckDestroyObject(player, colliderHit);
        }

        player.explosionEffect.transform.localPosition = explosionDistance;
    }

    void CheckDamageEnemy(PlayerFSM player, Collider2D colliderHit) {
        bool hitEnemy = colliderHit.gameObject.CompareTag("Enemy");
        if (hitEnemy) {
            Enemy enemy = colliderHit.GetComponent<Enemy>();
            float damage = enemy.maxHealth * explosionDamageRate;
            enemy.TakeDamage(damage);
        }
    }

    void CheckDestroyObject(PlayerFSM player, Collider2D colliderHit) {
        bool hitObstacle = colliderHit.gameObject.layer == LayerMask.NameToLayer("Obstacles");

        bool hitProjectile = colliderHit.gameObject.CompareTag("Projectile");
        if (hitObstacle || hitProjectile) {
            MonoBehaviour.Destroy(colliderHit.gameObject);
        }
    }

    void InputBuffer() {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
    }
}
