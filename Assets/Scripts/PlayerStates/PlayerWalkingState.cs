using UnityEngine;

public class PlayerWalkingState : PlayerBaseState {
    public override void EnterState(PlayerFSM player) {
        player.animator.Play("PlayerWalk");
    }

    public override void Update(PlayerFSM player) {
        float xInput = Input.GetAxisRaw("Horizontal");
        Movement(xInput, player);

        if (!player.isGrounded) {
            player.TransitionToState(player.FallingState);
        }

        if (xInput == 0) {
            player.TransitionToState(player.GroundedState);
        }

        if (Input.GetButtonDown("Jump")) {
            player.TransitionToState(player.JumpingState);
        }
    }

    void Movement(float xInput, PlayerFSM player) {
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
