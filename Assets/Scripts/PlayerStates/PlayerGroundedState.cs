using UnityEngine;

public class PlayerGroundedState : PlayerBaseState {
    public override void EnterState(PlayerFSM player) {
        player.rb.velocity = Vector2.zero;
        player.animator.Play("PlayerIdle");
    }

    public override void Update(PlayerFSM player) {
        if (!player.isGrounded) {
            player.TransitionToState(player.FallingState);
        }

        if (Input.GetButtonDown("Jump")) {
            player.TransitionToState(player.JumpingState);
        }

        float xInput = Input.GetAxisRaw("Horizontal");
        if (xInput != 0) {
            player.TransitionToState(player.WalkingState);
        }
    }
}
