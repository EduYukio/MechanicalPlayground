using UnityEngine;

public class PlayerJumpingState : PlayerBaseState {
    private bool playerIsFalling = false;
    private bool playerReleasedJumpButton = false;

    public override void EnterState(PlayerFSM player) {
        player.animator.Play("PlayerJump");
        player.rb.velocity = new Vector2(player.rb.velocity.x, player.config.jumpForce);
    }

    public override void Update(PlayerFSM player) {
        float xInput = Input.GetAxisRaw("Horizontal");
        AirMovement(xInput, player);

        CheckTransitionToFalling(player);
    }

    void CheckTransitionToFalling(PlayerFSM player) {
        playerIsFalling = player.rb.velocity.y < 0;
        playerReleasedJumpButton = player.rb.velocity.y > 0 && !Input.GetButton("Jump");

        if (playerIsFalling || playerReleasedJumpButton) {
            player.TransitionToState(player.FallingState);
        }
    }

    void AirMovement(float xInput, PlayerFSM player) {
        int direction = 0;
        if (xInput > 0) {
            direction = 1;
            player.lastDirection = direction;
        }
        else if (xInput < 0) {
            direction = -1;
            player.lastDirection = direction;
        }

        player.rb.velocity = new Vector2(direction * player.config.moveSpeed, player.rb.velocity.y);
    }
}
