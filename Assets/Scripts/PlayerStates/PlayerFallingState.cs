using System.Collections;
using UnityEngine;

public class PlayerFallingState : PlayerBaseState {
    public override void EnterState(PlayerFSM player) {
        PlayAnimationIfCan(player);
    }

    public override void Update(PlayerFSM player) {
        BetterFalling(player);
        base.ProcessMovementInput(player);

        base.CheckTransitionToGrounded(player);
        base.CheckTransitionToDoubleJump(player);
        base.CheckTransitionToDashing(player);
        base.CheckTransitionToWallSliding(player);
        base.CheckTransitionToWallJumping(player);
        base.CheckTransitionToAttacking(player);
    }

    void BetterFalling(PlayerFSM player) {
        bool playerIsFalling = player.rb.velocity.y < 0;
        bool playerStoppedJumping = player.rb.velocity.y > 0 && !Input.GetButton("Jump");

        if (playerIsFalling) {
            player.rb.velocity += Vector2.up * Physics2D.gravity.y * (player.config.fallMultiplier - 1) * Time.deltaTime;
        }
        else if (playerStoppedJumping) {
            player.rb.velocity += Vector2.up * Physics2D.gravity.y * (player.config.lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    void PlayAnimationIfCan(PlayerFSM player) {
        if (IsPlayingAnimation("PlayerDoubleJump", player)) return;
        if (IsPlayingAnimation("PlayerAttack", player)) return;

        player.animator.Play("PlayerFall");
    }
}
