using UnityEngine;

public class PlayerWalkingState : PlayerBaseState {
    public override void EnterState(PlayerFSM player) {
        PlayAnimationIfCan(player);
    }

    public override void Update(PlayerFSM player) {
        player.coyoteTimer = player.config.startCoyoteDurationTime;
        base.ProcessMovementInput(player);

        if (CheckTransitionToGrounded(player)) return;
        if (base.CheckTransitionToFalling(player)) return;
        if (base.CheckTransitionToJumping(player)) return;
        if (base.CheckTransitionToDashing(player)) return;
        if (base.CheckTransitionToAttacking(player)) return;
        if (base.CheckTransitionToBlinking(player)) return;
    }

    public override bool CheckTransitionToGrounded(PlayerFSM player) {
        if (!player.isGrounded) return false;

        float xInput = Input.GetAxisRaw("Horizontal");
        if (xInput == 0) {
            player.TransitionToState(player.GroundedState);
            return true;
        }
        return false;
    }

    private void PlayAnimationIfCan(PlayerFSM player) {
        if (IsPlayingAnimation("PlayerAttack", player)) return;

        player.animator.Play("PlayerWalk");
    }
}
