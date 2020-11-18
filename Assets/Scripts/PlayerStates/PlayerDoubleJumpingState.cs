using UnityEngine;

public class PlayerDoubleJumpingState : PlayerBaseState {
    private bool playerIsFalling = false;
    private bool playerReleasedJumpButton = false;

    public override void EnterState(PlayerFSM player) {
        player.animator.Play("PlayerDoubleJump");
        player.canDoubleJump = false;
        player.rb.velocity = new Vector2(player.rb.velocity.x, player.config.doubleJumpForce);
    }

    public override void Update(PlayerFSM player) {
        ProcessMovementInput(player);

        CheckTransitionToFalling(player);
    }

    void CheckTransitionToFalling(PlayerFSM player) {
        playerIsFalling = player.rb.velocity.y < 0;
        playerReleasedJumpButton = player.rb.velocity.y > 0 && !Input.GetButton("Jump");

        if (playerIsFalling || playerReleasedJumpButton) {
            player.TransitionToState(player.FallingState);
        }
    }
}
