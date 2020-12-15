using UnityEngine;

public class PlayerAttackingState : PlayerBaseState {
    public LayerMask enemyLayers;
    public float attackTimer;
    GameObject slashEffect;
    float xInput;
    float yInput;

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
        slashEffect = player.slashEffect;
        enemyLayers = LayerMask.GetMask("Enemies");
        player.attackCooldownTimer = player.config.startAttackCooldownTime;
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
    }

    void AttackAction(PlayerFSM player) {
        Vector3 attackDirection = base.GetFourDirectionalInput(player, xInput, yInput);
        Vector3 attackPosition = CalculateAttackPosition(player, attackDirection);
        PositionSlashEffect(attackPosition, attackDirection);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPosition, player.config.attackRadius, enemyLayers);
        foreach (Collider2D enemy in hitEnemies) {
            enemy.GetComponent<Enemy>()?.TakeDamage(player.config.attackDamage);
        }
    }

    Vector3 CalculateAttackPosition(PlayerFSM player, Vector3 direction) {
        float distance = player.config.attackDistance;

        Vector3 playerPos = player.transform.position;
        Vector3 attackPosition = playerPos + (direction * distance);

        return attackPosition;
    }

    void PositionSlashEffect(Vector3 attackPosition, Vector3 direction) {
        slashEffect.transform.position = attackPosition + new Vector3(0f, -0.13f, 0f);

        if (direction == Vector3.right) {
            slashEffect.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
        else if (direction == Vector3.up) {
            slashEffect.transform.eulerAngles = new Vector3(0f, 0f, 90f);
        }
        else if (direction == Vector3.left) {
            slashEffect.transform.eulerAngles = new Vector3(0f, 0f, 180f);
        }
        else if (direction == Vector3.down) {
            slashEffect.transform.eulerAngles = new Vector3(0f, 0f, -90f);
        }
    }
}
