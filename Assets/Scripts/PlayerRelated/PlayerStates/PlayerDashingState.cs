using UnityEngine;

public class PlayerDashingState : PlayerBaseState {
    public float dashTimer;
    private float originalGravity;
    private bool isEthereal;

    public override void EnterState(PlayerFSM player) {
        player.animator.Play("PlayerDash");
        Setup(player);
        DashAction(player);
    }

    public override void Update(PlayerFSM player) {
        CheckForAirJumpInputBuffer(player);

        if (dashTimer > 0) {
            dashTimer -= Time.deltaTime;
            return;
        }

        StopDashing(player);
        if (base.CheckTransitionToGrounded(player)) return;
        if (base.CheckTransitionToDoubleJumping(player)) return;
        if (CheckTransitionToWallSliding(player)) return;
        if (CheckTransitionToFalling(player)) return;
    }

    void Setup(PlayerFSM player) {
        dashTimer = player.config.startDashDurationTime;
        isEthereal = player.mechanics.IsEnabled("EtherealDash");
    }

    void DashAction(PlayerFSM player) {
        originalGravity = player.rb.gravityScale;
        player.rb.gravityScale = 0f;
        player.rb.velocity = new Vector2(player.lastDirection * player.config.dashSpeed, 0f);
        if (isEthereal) {
            player.spriteRenderer.color = new Color(1, 1, 1, player.config.etherealTransparency);
            player.gameObject.layer = LayerMask.NameToLayer("PlayerEthereal");
        }
    }

    void StopDashing(PlayerFSM player) {
        player.rb.gravityScale = originalGravity;
        player.rb.velocity = Vector2.zero;
        player.canDash = false;
        player.dashCooldownTimer = player.config.startDashCooldownTime;
        if (isEthereal) {
            player.spriteRenderer.color = new Color(1, 1, 1, 1);
            player.gameObject.layer = LayerMask.NameToLayer("Player"); ;
        }
    }

    void CheckForAirJumpInputBuffer(PlayerFSM player) {
        if (Input.GetButtonDown("Jump")) {
            player.airJumpInputBuffer = player.config.startAirJumpInputBuffer;
        }
    }

    public override bool CheckTransitionToWallSliding(PlayerFSM player) {
        if (player.isTouchingWall) {
            player.TransitionToState(player.WallSlidingState);
            return true;
        }
        return false;
    }

    public override bool CheckTransitionToFalling(PlayerFSM player) {
        if (!player.isTouchingWall && !player.isGrounded) {
            player.TransitionToState(player.FallingState);
            return true;
        }
        return false;
    }
}
