using UnityEngine;

public class PlayerGroundedState : PlayerBaseState {
    public override void EnterState(PlayerFSM player) {
        Setup(player);
        GroundedAction(player);
    }

    public override void Update(PlayerFSM player) {
        PlayAnimationIfCan(player);

        if (base.CheckTransitionToFalling(player)) return;
        if (base.CheckTransitionToJumping(player)) return;
        if (base.CheckTransitionToWalking(player)) return;
        if (base.CheckTransitionToDashing(player)) return;
        if (base.CheckTransitionToAttacking(player)) return;
        if (base.CheckTransitionToBlinking(player)) return;
    }

    void Setup(PlayerFSM player) {
        player.canDoubleJump = true;
        player.canDash = true;
    }

    void GroundedAction(PlayerFSM player) {
        player.rb.velocity = Vector2.zero;
    }

    private void PlayAnimationIfCan(PlayerFSM player) {
        if (IsPlayingAnimation("PlayerIdle", player)) return;
        if (IsPlayingAnimation("PlayerAttack", player)) return;
        if (IsPlayingAnimation("PlayerAppear", player)) return;

        player.animator.Play("PlayerIdle");
    }
}
