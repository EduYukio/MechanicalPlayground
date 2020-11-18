using UnityEngine;

public class PlayerFallingState : PlayerBaseState {
    private bool playerIsFalling = false;
    private bool playerReleasedJumpButton = false;

    public override void EnterState(PlayerFSM player) {
        player.animator.Play("PlayerFall");
    }

    public override void Update(PlayerFSM player) {
        BetterFalling(player);

        float xInput = Input.GetAxisRaw("Horizontal");
        AirMovement(xInput, player);

        if (player.isGrounded) {
            player.TransitionToState(player.GroundedState);
        }
    }

    void BetterFalling(PlayerFSM player) {
        playerIsFalling = player.rb.velocity.y < 0;
        playerReleasedJumpButton = player.rb.velocity.y > 0 && !Input.GetButton("Jump");

        if (playerIsFalling) {
            player.rb.velocity += Vector2.up * Physics2D.gravity.y * (player.config.fallMultiplier - 1) * Time.deltaTime;
        }
        else if (playerReleasedJumpButton) {
            player.rb.velocity += Vector2.up * Physics2D.gravity.y * (player.config.lowJumpMultiplier - 1) * Time.deltaTime;
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
