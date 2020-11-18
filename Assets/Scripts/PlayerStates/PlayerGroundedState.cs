using UnityEngine;

public class PlayerGroundedState : PlayerBaseState {
    public override void EnterState(PlayerFSM player) {
        player.canDoubleJump = true;
        player.rb.velocity = Vector2.zero;
        player.animator.Play("PlayerIdle");
    }

    public override void Update(PlayerFSM player) {
        CheckTransitionToFalling(player);
        CheckTransitionToJumping(player);
        CheckTransitionToWalking(player);
    }

    void CheckTransitionToFalling(PlayerFSM player) {
        if (!player.isGrounded) {
            player.TransitionToState(player.FallingState);
        }
    }

    void CheckTransitionToJumping(PlayerFSM player) {
        if (Input.GetButtonDown("Jump")) {
            player.TransitionToState(player.JumpingState);
        }
    }

    void CheckTransitionToWalking(PlayerFSM player) {
        float xInput = Input.GetAxisRaw("Horizontal");
        if (xInput != 0) {
            player.TransitionToState(player.WalkingState);
        }
    }
}
