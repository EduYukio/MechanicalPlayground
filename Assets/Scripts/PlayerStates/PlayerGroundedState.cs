using UnityEngine;

public class PlayerGroundedState : PlayerBaseState {
    public override void EnterState(PlayerFSM player) {
        player.animator.Play("PlayerIdle");
    }

    public override void Update(PlayerFSM player) {
        if (!player.isGrounded) {
            player.TransitionToState(player.FallingState);
        }

        if (Input.GetButtonDown("Jump")) {
            player.TransitionToState(player.JumpingState);
        }
    }
}
