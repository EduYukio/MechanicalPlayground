using UnityEngine;

public class PlayerJumpingState : PlayerBaseState {
    private bool leftGround;
    private string[] waitAnimations;

    public override void EnterState(PlayerFSM player) {
        Setup(player);
        PlayParticles(player);
        JumpAction(player);
    }

    public override void Update(PlayerFSM player) {
        Helper.PlayAnimationIfPossible("PlayerJump", player.animator, waitAnimations);
        base.ProcessHorizontalMoveInput(player);
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
        waitAnimations = new string[] { "PlayerJump", "PlayerAttacking", "PlayerAttackingBoosted", "PlayerAppear" };
    }

    private void PlayParticles(PlayerFSM player) {
        player.jumpParticles.Play();
    }

    private void JumpAction(PlayerFSM player) {
        player.rb.velocity = new Vector2(player.rb.velocity.x, player.config.jumpForce);
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
