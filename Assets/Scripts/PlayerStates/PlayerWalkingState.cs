using UnityEngine;

public class PlayerWalkingState : PlayerBaseState {
    public override void EnterState(PlayerFSM player) {
        player.animator.Play("PlayerWalk");
    }

    public override void Update(PlayerFSM player) {
        ProcessMovementInput(player);

        CheckTransitionToFalling(player);
        CheckTransitionToJumping(player);
        CheckTransitionToGrounded(player);
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

    void CheckTransitionToGrounded(PlayerFSM player) {
        float xInput = Input.GetAxisRaw("Horizontal");
        if (xInput == 0) {
            player.TransitionToState(player.GroundedState);
        }
    }
}
