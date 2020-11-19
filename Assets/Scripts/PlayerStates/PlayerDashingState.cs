using UnityEngine;

public class PlayerDashingState : PlayerBaseState {
    public float dashTimer;
    private int dashDirection;

    public override void EnterState(PlayerFSM player) {
        player.animator.Play("PlayerDash");
        dashTimer = player.config.startDashTime;
        dashDirection = player.lastDirection;
    }

    public override void Update(PlayerFSM player) {
        if (dashTimer > 0) {
            DashAction(player);
            return;
        }

        StopDashing(player);
        CheckTransitionToGrounded(player);
        CheckTransitionToFalling(player);
    }

    void CheckTransitionToFalling(PlayerFSM player) {
        if (!player.isGrounded) {
            player.TransitionToState(player.FallingState);
        }
    }

    void CheckTransitionToGrounded(PlayerFSM player) {
        if (player.isGrounded) {
            player.TransitionToState(player.GroundedState);
        }
    }

    // void CheckTransitionToWallSliding(PlayerFSM player) {
    // }

    void DashAction(PlayerFSM player) {
        player.rb.velocity = new Vector2(dashDirection * player.config.dashSpeed, 0f);
        dashTimer -= Time.deltaTime;
    }

    public void StopDashing(PlayerFSM player) {
        player.rb.velocity = Vector2.zero;
        player.canDash = false;
        player.dashCooldownTimer = player.config.startDashCooldownTime;
    }
}
