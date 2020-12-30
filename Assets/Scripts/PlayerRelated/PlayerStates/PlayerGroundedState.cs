using UnityEngine;

public class PlayerGroundedState : PlayerBaseState {
    public override void EnterState(PlayerFSM player) {
        Setup(player);
        GroundedAction(player);
    }

    public override void Update(PlayerFSM player) {
        PlayAnimationIfCan(player);

        if (base.CheckTransitionToGunBoots(player)) return;
        if (base.CheckTransitionToFalling(player)) return;
        if (base.CheckTransitionToJumping(player)) return;
        if (base.CheckTransitionToWalking(player)) return;
        if (base.CheckTransitionToDashing(player)) return;
        if (base.CheckTransitionToAttacking(player)) return;
        if (base.CheckTransitionToBlinking(player)) return;
        if (base.CheckTransitionToShotgunning(player)) return;
    }

    void Setup(PlayerFSM player) {
        player.canDoubleJump = true;
        player.canDash = true;
    }

    void GroundedAction(PlayerFSM player) {
        player.rb.velocity = Vector2.zero;
    }

    private void PlayAnimationIfCan(PlayerFSM player) {
        if (Helper.IsPlayingAnimation("PlayerIdle", player.animator)) return;
        if (Helper.IsPlayingAnimation("PlayerAttacking", player.animator)) return;
        if (Helper.IsPlayingAnimation("PlayerAttackingBoosted", player.animator)) return;
        if (Helper.IsPlayingAnimation("PlayerAppear", player.animator)) return;
        if (Helper.IsPlayingAnimation("PlayerDisappear", player.animator)) return;
        if (Helper.IsPlayingAnimation("PlayerShotgunning", player.animator)) return;

        player.animator.Play("PlayerIdle");
    }
}
