using UnityEngine;

public class PlayerWalkingState : PlayerBaseState {
    public override void EnterState(PlayerFSM player) {
        player.animator.Play("PlayerWalk");
    }

    public override void Update(PlayerFSM player) {
        base.ProcessMovementInput(player);

        CheckTransitionToGrounded(player);
        base.CheckTransitionToFalling(player);
        base.CheckTransitionToJumping(player);
        base.CheckTransitionToDashing(player);
    }

    public override void CheckTransitionToGrounded(PlayerFSM player) {
        if (!player.isGrounded) return;

        float xInput = Input.GetAxisRaw("Horizontal");
        if (xInput == 0) {
            player.TransitionToState(player.GroundedState);
        }
    }
}
