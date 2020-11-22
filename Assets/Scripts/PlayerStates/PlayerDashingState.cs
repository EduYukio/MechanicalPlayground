using UnityEngine;

public class PlayerDashingState : PlayerBaseState {
    public float dashTimer;
    private int dashDirection;

    public override void EnterState(PlayerFSM player) {
        player.animator.Play("PlayerDash");
        dashTimer = player.config.startDashDurationTime;
        dashDirection = player.lastDirection;
    }

    public override void Update(PlayerFSM player) {
        if (dashTimer > 0) {
            DashAction(player);
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
        player.rb.velocity = new Vector2(dashDirection * player.config.dashSpeed, 0f);
        dashTimer -= Time.deltaTime;
    }

    void StopDashing(PlayerFSM player) {
        player.rb.velocity = Vector2.zero;
        player.canDash = false;
        player.dashCooldownTimer = player.config.startDashCooldownTime;
    }
}
