using UnityEngine;

public class PlayerGroundedState : PlayerBaseState {
    public override void EnterState(PlayerFSM player) {
        PlayAnimationIfCan(player);
        GroundedAction(player);
    }

    public override void Update(PlayerFSM player) {
        base.CheckTransitionToFalling(player);
        base.CheckTransitionToJumping(player);
        base.CheckTransitionToWalking(player);
        base.CheckTransitionToDashing(player);
        base.CheckTransitionToAttacking(player);
    }

    void GroundedAction(PlayerFSM player) {
        player.rb.velocity = Vector2.zero;
        player.canDoubleJump = true;
        player.canDash = true;
    }

    private void PlayAnimationIfCan(PlayerFSM player) {
        if (IsPlayingAnimation("PlayerAttack", player)) return;

        player.animator.Play("PlayerIdle");
    }
}
