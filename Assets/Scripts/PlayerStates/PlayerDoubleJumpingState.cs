using UnityEngine;

public class PlayerDoubleJumpingState : PlayerBaseState {
    public override void EnterState(PlayerFSM player) {
        player.animator.Play("PlayerDoubleJump");
        DoubleJumpAction(player);
    }

    public override void Update(PlayerFSM player) {
        base.ProcessMovementInput(player);

        if (base.CheckTransitionToFalling(player)) return;
        if (base.CheckTransitionToDashing(player)) return;
        if (base.CheckTransitionToAttacking(player)) return;
    }

    void DoubleJumpAction(PlayerFSM player) {
        player.rb.velocity = new Vector2(player.rb.velocity.x, player.config.doubleJumpForce);
        player.canDoubleJump = false;
    }
}
