using UnityEngine;

public class PlayerAttackingState : PlayerBaseState {
    public LayerMask enemyLayers;
    public float attackTimer;
    Vector3 attackDirection;
    bool isBoosted;

    public override void EnterState(PlayerFSM player) {
        Setup(player);
        AttackAction(player);
        if (isBoosted) {
            player.animator.Play("PlayerAttackingBoosted");
        }
        else {
            player.animator.Play("PlayerAttacking");
        }
    }

    public override void Update(PlayerFSM player) {
        if (base.CheckTransitionToWalking(player)) return;
        if (base.CheckTransitionToGrounded(player)) return;
        if (base.CheckTransitionToFalling(player)) return;
    }

    void Setup(PlayerFSM player) {
        enemyLayers = LayerMask.GetMask("Enemies");
        player.attackCooldownTimer = player.config.startAttackCooldownTime;
        isBoosted = player.mechanics.IsEnabled("Range Boost");
        attackDirection = CalculateDirection(player);
        PositionSlashEffect(player, isBoosted, attackDirection);
    }

    Vector3 CalculateDirection(PlayerFSM player) {
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");
        return base.GetFourDirectionalInput(player, xInput, yInput);
    }


    void AttackAction(PlayerFSM player) {
        if (isBoosted) {
            player.boostedSlash.SetActive(true);
            player.normalSlash.SetActive(false);
        }
        else {
            player.normalSlash.SetActive(true);
            player.boostedSlash.SetActive(false);
        }
    }

    void PositionSlashEffect(PlayerFSM player, bool isBoosted, Vector3 direction) {
        if (isBoosted) {
            if (direction == Vector3.right) {
                player.boostedSlash.transform.eulerAngles = new Vector3(0, 0f, 0f);
                player.boostedSlash.transform.localPosition = new Vector3(1, -0.16f, 0f);
            }
            else if (direction == Vector3.up) {
                player.boostedSlash.transform.eulerAngles = new Vector3(0, 0f, 90f);
                player.boostedSlash.transform.localPosition = new Vector3(0, 0.95f, 0f);
            }
            else if (direction == Vector3.left) {
                player.boostedSlash.transform.eulerAngles = new Vector3(0, 0f, 180f);
                player.boostedSlash.transform.localPosition = new Vector3(-1, -0.16f, 0f);
            }
            else if (direction == Vector3.down) {
                player.boostedSlash.transform.eulerAngles = new Vector3(0, 0f, -90f);
                player.boostedSlash.transform.localPosition = new Vector3(0, -1.23f, 0f);
            }
        }
        else {
            if (direction == Vector3.right) {
                player.normalSlash.transform.eulerAngles = new Vector3(0, 0f, 0f);
                player.normalSlash.transform.localPosition = new Vector3(0.7f, -0.15f, 0f);
            }
            else if (direction == Vector3.up) {
                player.normalSlash.transform.eulerAngles = new Vector3(0, 0f, 90f);
                player.normalSlash.transform.localPosition = new Vector3(0, 0.59f, 0);
            }
            else if (direction == Vector3.left) {
                player.normalSlash.transform.eulerAngles = new Vector3(0, 0f, 180f);
                player.normalSlash.transform.localPosition = new Vector3(-0.7f, -0.15f, 0f);
            }
            else if (direction == Vector3.down) {
                player.normalSlash.transform.eulerAngles = new Vector3(0, 0f, -90f);
                player.normalSlash.transform.localPosition = new Vector3(0, -0.86f, 0f);
            }
        }
    }
}
