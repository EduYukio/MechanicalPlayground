using UnityEngine;

public class PlayerJumpingState : PlayerBaseState {
    public override void EnterState(PlayerFSM player) {
        player.animator.Play("PlayerJump");
        JumpAction(player);
    }

    public override void Update(PlayerFSM player) {
        base.ProcessMovementInput(player);

        if (base.CheckTransitionToFalling(player)) return;
        if (base.CheckTransitionToDashing(player)) return;
        if (base.CheckTransitionToDoubleJumping(player)) return;
        if (base.CheckTransitionToAttacking(player)) return;
    }

    void JumpAction(PlayerFSM player) {
        player.coyoteTimer = 0;
        player.bunnyHopTimer = 0;
        player.rb.velocity = new Vector2(player.rb.velocity.x, player.config.jumpForce);
    }
}
