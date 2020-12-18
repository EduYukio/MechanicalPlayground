using UnityEngine;

public class PlayerPogoingState : PlayerBaseState {
    public override void EnterState(PlayerFSM player) {
        Setup(player);
        PogoAction(player);
    }

    public override void Update(PlayerFSM player) {
        PlayAnimationIfCan(player);
        base.ProcessMovementInput(player);

        if (base.CheckTransitionToWallSliding(player)) return;
        if (CheckTransitionToFalling(player)) return;
        if (base.CheckTransitionToDashing(player)) return;
        if (base.CheckTransitionToDoubleJumping(player)) return;
        if (base.CheckTransitionToAttacking(player)) return;
        if (base.CheckTransitionToBlinking(player)) return;
        if (base.CheckTransitionToShotgunning(player)) return;
    }

    void Setup(PlayerFSM player) {
        player.canDoubleJump = true;
        player.canDash = true;
    }

    void PogoAction(PlayerFSM player) {
        player.rb.velocity = new Vector2(player.rb.velocity.x, player.config.pogoForce);
    }

    private void PlayAnimationIfCan(PlayerFSM player) {
        if (IsPlayingAnimation("PlayerJump", player)) return;
        if (IsPlayingAnimation("PlayerAttacking", player)) return;
        if (IsPlayingAnimation("PlayerAttackingBoosted", player)) return;

        player.animator.Play("PlayerJump");
    }

    public override bool CheckTransitionToFalling(PlayerFSM player) {
        if (player.isGrounded) return false;

        bool playerIsFalling = player.rb.velocity.y <= 0;

        if (playerIsFalling) {
            player.TransitionToState(player.FallingState);
            return true;
        }

        return false;
    }
}
