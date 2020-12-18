using UnityEngine;

public class PlayerJumpingState : PlayerBaseState {
    public override void EnterState(PlayerFSM player) {
        Setup(player);
        JumpAction(player);
    }

    public override void Update(PlayerFSM player) {
        PlayAnimationIfCan(player);
        base.ProcessMovementInput(player);

        if (base.CheckTransitionToWallSliding(player)) return;
        if (base.CheckTransitionToFalling(player)) return;
        if (base.CheckTransitionToDashing(player)) return;
        if (base.CheckTransitionToDoubleJumping(player)) return;
        if (base.CheckTransitionToAttacking(player)) return;
        if (base.CheckTransitionToBlinking(player)) return;
        if (base.CheckTransitionToShotgunning(player)) return;
    }

    void Setup(PlayerFSM player) {
        player.coyoteTimer = 0;
        player.bunnyHopTimer = 0;
    }

    void JumpAction(PlayerFSM player) {
        player.rb.velocity = new Vector2(player.rb.velocity.x, player.config.jumpForce);
    }

    private void PlayAnimationIfCan(PlayerFSM player) {
        if (IsPlayingAnimation("PlayerJump", player)) return;
        if (IsPlayingAnimation("PlayerAttacking", player)) return;
        if (IsPlayingAnimation("PlayerAttackingBoosted", player)) return;

        player.animator.Play("PlayerJump");
    }
}
