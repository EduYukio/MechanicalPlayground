using UnityEngine;

public class PlayerFallingState : PlayerBaseState {
    private string[] waitAnimations;

    public override void EnterState(PlayerFSM player) {
        Setup();
    }

    public override void Update(PlayerFSM player) {
        Helper.PlayAnimationIfPossible("PlayerFall", player.animator, waitAnimations);
        BetterFalling(player);
        CheckForBunnyHop(player);
        base.ProcessHorizontalMoveInput(player);

        if (CheckTransitionToJumping(player)) return;
        if (base.CheckTransitionToGunBoots(player)) return;
        if (base.CheckTransitionToWallJumping(player)) return;
        if (base.CheckTransitionToDoubleJumping(player)) return;
        if (CheckTransitionToGrounded(player)) return;
        if (base.CheckTransitionToDashing(player)) return;
        if (base.CheckTransitionToWallSliding(player)) return;
        if (base.CheckTransitionToAttacking(player)) return;
        if (base.CheckTransitionToBlinking(player)) return;
        if (base.CheckTransitionToExploding(player)) return;
    }

    private void Setup() {
        waitAnimations = new string[] { "PlayerFall", "PlayerDoubleJump", "PlayerAttacking", "PlayerAttackingBoosted", "PlayerAppear", "PlayerExploding" };
    }

    private void BetterFalling(PlayerFSM player) {
        if (player.rb.velocity.y < player.config.maxFallSpeed) {
            player.rb.velocity = new Vector2(player.rb.velocity.x, player.config.maxFallSpeed);
            return;
        }

        bool playerIsFalling = player.rb.velocity.y < 0;
        bool playerStoppedJumping = player.rb.velocity.y > 0 && !Input.GetButton("Jump");

        if (playerIsFalling) {
            float fallMultiplier = player.config.fallMultiplier - 1;
            player.rb.velocity += Vector2.up * Physics2D.gravity.y * fallMultiplier * Time.deltaTime;
        }
        else if (playerStoppedJumping) {
            float lowJumpMultiplier = player.config.lowJumpMultiplier - 1;
            player.rb.velocity += Vector2.up * Physics2D.gravity.y * lowJumpMultiplier * Time.deltaTime;
        }
    }

    private void CheckForBunnyHop(PlayerFSM player) {
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

    public override bool CheckTransitionToGrounded(PlayerFSM player) {
        if (player.isGrounded) {
            Manager.audio.Play("Landing");
            player.leftSideParticles.Play();
            player.rightSideParticles.Play();

            player.TransitionToState(player.GroundedState);
            return true;
        }

        return false;
    }
}
