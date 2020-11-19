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
        base.CheckTransitionToDoubleJump(player);
    }

    void JumpAction(PlayerFSM player) {
        player.rb.velocity = new Vector2(player.rb.velocity.x, player.config.jumpForce);
    }
}
