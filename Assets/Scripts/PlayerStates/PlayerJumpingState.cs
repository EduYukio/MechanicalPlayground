using UnityEngine;

public class PlayerJumpingState : PlayerBaseState {
    public override void EnterState(PlayerFSM player) {
        player.animator.Play("PlayerJump");
        JumpAction(player);
    }

    public override void Update(PlayerFSM player) {
        base.ProcessMovementInput(player);

        base.CheckTransitionToFalling(player);
        base.CheckTransitionToDashing(player);
        base.CheckTransitionToDoubleJumping(player);
        base.CheckTransitionToAttacking(player);
    }

    void JumpAction(PlayerFSM player) {
        player.coyoteTimer = 0;
        player.bunnyHopTimer = 0;
        player.rb.velocity = new Vector2(player.rb.velocity.x, player.config.jumpForce);
    }
}
