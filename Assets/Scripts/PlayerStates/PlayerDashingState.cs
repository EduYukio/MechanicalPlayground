using UnityEngine;

public class PlayerDashingState : PlayerBaseState {
    public float dashTimer;
    private int dashDirection;
    private float originalGravity;

    public override void EnterState(PlayerFSM player) {
        player.animator.Play("PlayerDash");
        DashAction(player);
    }

    public override void Update(PlayerFSM player) {
        if (dashTimer > 0) {
            dashTimer -= Time.deltaTime;
            return;
        }

        StopDashing(player);
        if (base.CheckTransitionToGrounded(player)) return;
        if (CheckTransitionToWallSliding(player)) return;
        if (CheckTransitionToFalling(player)) return;
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

    void DashAction(PlayerFSM player) {
        dashTimer = player.config.startDashDurationTime;
        dashDirection = player.lastDirection;
        originalGravity = player.rb.gravityScale;
        player.rb.gravityScale = 0f;
        player.rb.velocity = new Vector2(dashDirection * player.config.dashSpeed, 0f);
    }

    void StopDashing(PlayerFSM player) {
        player.rb.gravityScale = originalGravity;
        player.rb.velocity = Vector2.zero;
        player.canDash = false;
        player.dashCooldownTimer = player.config.startDashCooldownTime;
    }
}
