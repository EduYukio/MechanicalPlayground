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
        base.CheckTransitionToFalling(player);
        base.CheckTransitionToGrounded(player);
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
