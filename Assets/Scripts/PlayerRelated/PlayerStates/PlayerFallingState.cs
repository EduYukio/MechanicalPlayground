using System.Collections;
using UnityEngine;

public class PlayerFallingState : PlayerBaseState {
    public override void EnterState(PlayerFSM player) {
    }

    public override void Update(PlayerFSM player) {
        PlayAnimationIfCan(player);
        BetterFalling(player);
        CheckForBunnyHop(player);
        base.ProcessMovementInput(player);

        if (CheckTransitionToJumping(player)) return;
        if (base.CheckTransitionToDoubleJumping(player)) return;
        if (base.CheckTransitionToGrounded(player)) return;
        if (base.CheckTransitionToDashing(player)) return;
        if (base.CheckTransitionToWallSliding(player)) return;
        if (base.CheckTransitionToWallJumping(player)) return;
        if (base.CheckTransitionToAttacking(player)) return;
        if (base.CheckTransitionToBlinking(player)) return;
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
        if (IsPlayingAnimation("PlayerFall", player)) return;
        if (IsPlayingAnimation("PlayerDoubleJump", player)) return;
        if (IsPlayingAnimation("PlayerAttack", player)) return;
        if (IsPlayingAnimation("PlayerAppear", player)) return;

        player.animator.Play("PlayerFall");
    }

    void CheckForBunnyHop(PlayerFSM player) {
        if (player.coyoteTimer > 0) return;

        if (Input.GetButtonDown("Jump")) {
            player.bunnyHopTimer = player.config.startBunnyHopDurationTime;
        }
    }

    public override bool CheckTransitionToJumping(PlayerFSM player) {
        if (player.coyoteTimer > 0) {
            return base.CheckTransitionToJumping(player);
        }

        return false;
    }
}
