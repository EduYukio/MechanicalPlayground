using UnityEngine;

public class PlayerJumpingState : PlayerBaseState {
    private bool leftGround;

    public override void EnterState(PlayerFSM player) {
        Setup(player);
        JumpAction(player);
    }

    public override void Update(PlayerFSM player) {
        PlayAnimationIfCan(player);
        base.ProcessMovementInput(player);
        CheckIfLeftGround(player);

        if (CheckTransitionToGrounded(player)) return;
        if (base.CheckTransitionToGunBoots(player)) return;
        if (base.CheckTransitionToWallSliding(player)) return;
        if (base.CheckTransitionToFalling(player)) return;
        if (base.CheckTransitionToDashing(player)) return;
        if (base.CheckTransitionToDoubleJumping(player)) return;
        if (base.CheckTransitionToAttacking(player)) return;
        if (base.CheckTransitionToBlinking(player)) return;
        if (base.CheckTransitionToExploding(player)) return;
    }

    private void Setup(PlayerFSM player) {
        leftGround = false;
        player.coyoteTimer = 0;
        player.bunnyHopTimer = 0;
        player.jumpParticles.Play();
    }

    private void JumpAction(PlayerFSM player) {
        player.rb.velocity = new Vector2(player.rb.velocity.x, player.config.jumpForce);
    }

    private void PlayAnimationIfCan(PlayerFSM player) {
        if (Helper.IsPlayingAnimation("PlayerJump", player.animator)) return;
        if (Helper.IsPlayingAnimation("PlayerAttacking", player.animator)) return;
        if (Helper.IsPlayingAnimation("PlayerAttackingBoosted", player.animator)) return;
        if (Helper.IsPlayingAnimation("PlayerAppear", player.animator)) return;

        player.animator.Play("PlayerJump");
    }

    private void CheckIfLeftGround(PlayerFSM player) {
        if (leftGround) return;

        if (!player.isGrounded) {
            leftGround = true;
        }
    }

    public override bool CheckTransitionToGrounded(PlayerFSM player) {
        if (!leftGround) return false;
        return base.CheckTransitionToGrounded(player);
    }
}
