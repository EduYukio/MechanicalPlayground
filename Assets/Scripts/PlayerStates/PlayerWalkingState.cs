using UnityEngine;

public class PlayerWalkingState : PlayerBaseState {
    public override void EnterState(PlayerFSM player) {
        PlayAnimationIfCan(player);
    }

    public override void Update(PlayerFSM player) {
        player.coyoteTimer = player.config.startCoyoteDurationTime;
        base.ProcessMovementInput(player);

        CheckTransitionToGrounded(player);
        base.CheckTransitionToFalling(player);
        base.CheckTransitionToJumping(player);
        base.CheckTransitionToDashing(player);
        base.CheckTransitionToAttacking(player);
    }

    public override void CheckTransitionToGrounded(PlayerFSM player) {
        if (!player.isGrounded) return;

        float xInput = Input.GetAxisRaw("Horizontal");
        if (xInput == 0) {
            player.TransitionToState(player.GroundedState);
        }
    }

    private void PlayAnimationIfCan(PlayerFSM player) {
        if (IsPlayingAnimation("PlayerAttack", player)) return;

        player.animator.Play("PlayerWalk");
    }
}
