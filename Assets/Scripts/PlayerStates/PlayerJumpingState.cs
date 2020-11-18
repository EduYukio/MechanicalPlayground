using UnityEngine;

public class PlayerJumpingState : PlayerBaseState {
    private bool playerIsFalling = false;
    private bool playerReleasedJumpButton = false;

    public override void EnterState(PlayerFSM player) {
        player.animator.Play("PlayerJump");
        player.rb.velocity = new Vector2(player.rb.velocity.x, player.config.jumpForce);
    }

    public override void Update(PlayerFSM player) {
        playerIsFalling = player.rb.velocity.y < 0;
        playerReleasedJumpButton = player.rb.velocity.y > 0 && !Input.GetButton("Jump");

        if (playerIsFalling || playerReleasedJumpButton) {
            player.TransitionToState(player.FallingState);
        }
    }
}
