using UnityEngine;

public class PlayerAttackingState : PlayerBaseState {
    public Transform attackPoint;
    public LayerMask enemyLayers;
    public GameObject slashEffect;
    public float attackTimer;

    public override void EnterState(PlayerFSM player) {
        player.animator.Play("PlayerAttack");
        Setup(player);
        AttackAction(player);
    }

    public override void Update(PlayerFSM player) {
        // Pensar se vai ser instantâneo mesmo ou se vai durar um tempinho
        // if (attackTimer > 0) {
        //     attackTimer -= Time.deltaTime;
        //     return;
        // }

        if (base.CheckTransitionToWalking(player)) return;
        if (base.CheckTransitionToGrounded(player)) return;
        if (base.CheckTransitionToFalling(player)) return;
    }

    void Setup(PlayerFSM player) {
        attackPoint = GameObject.FindWithTag("AttackPoint").GetComponent<Transform>();
        slashEffect = GameObject.Find("Circular");
        enemyLayers = LayerMask.GetMask("Enemies");
        player.attackCooldownTimer = player.config.startAttackCooldownTime;
    }

    void AttackAction(PlayerFSM player) {
        Vector3 attackPosition = CalculateAttackPosition(player);
        PositionSlashEffect(attackPosition, player);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPosition, player.config.attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies) {
            enemy.GetComponent<Enemy>()?.TakeDamage(player.config.attackDamage);
        }
    }

    Vector3 CalculateAttackPosition(PlayerFSM player) {
        Vector3 playerPos = player.gameObject.transform.position;
        float xOffset = player.lastDirection * attackPoint.localPosition.x;
        Vector3 attackPosition = playerPos + new Vector3(xOffset, attackPoint.localPosition.y, 0);

        return attackPosition;
    }

    void PositionSlashEffect(Vector3 attackPosition, PlayerFSM player) {
        slashEffect.transform.position = attackPosition;
        slashEffect.GetComponent<SpriteRenderer>().flipX = player.spriteRenderer.flipX;
    }
}
