using UnityEngine;

public class PlayerGroundedState : PlayerBaseState {
    public override void EnterState(PlayerFSM player) {
        PlayAnimationIfCan(player);
        GroundedAction(player);
    }

    public override void Update(PlayerFSM player) {
        if (base.CheckTransitionToFalling(player)) return;
        if (base.CheckTransitionToJumping(player)) return;
        if (base.CheckTransitionToWalking(player)) return;
        if (base.CheckTransitionToDashing(player)) return;
        if (base.CheckTransitionToAttacking(player)) return;
        if (base.CheckTransitionToBlinking(player)) return;
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
