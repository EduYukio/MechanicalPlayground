using UnityEngine;

public class PlayerWallJumpingState : PlayerBaseState {
    private float wallJumpTimer;

    public override void EnterState(PlayerFSM player) {
        Setup(player);
        PlayAnimation(player);
        PlayParticles(player);
        WallJumpAction(player);
    }

    public override void FixedUpdate(PlayerFSM player) {
        if (base.CheckTransitionToDashing(player)) return;
        if (base.CheckTransitionToBlinking(player)) return;
        if (base.CheckTransitionToExploding(player)) return;
        if (base.CheckTransitionToGunBoots(player)) return;

        if (wallJumpTimer > 0) {
            wallJumpTimer -= Time.deltaTime;
            return;
        }

        if (CheckTransitionToWallSliding(player)) return;
        if (CheckTransitionToFalling(player)) return;
        if (base.CheckTransitionToGrounded(player)) return;
        if (base.CheckTransitionToAttacking(player)) return;
    }

    private void Setup(PlayerFSM player) {
        player.canDoubleJump = true;
        player.canDash = true;
        wallJumpTimer = player.config.startWallJumpDurationTime;
        base.SetLookingDirectionOppositeOfWall(player);
    }

    private void PlayAnimation(PlayerFSM player) {
        player.animator.Play("PlayerJump");
    }

    private void PlayParticles(PlayerFSM player) {
        player.jumpParticles.Play();
    }

    private void WallJumpAction(PlayerFSM player) {
        player.rb.velocity = new Vector2(player.lookingDirection * player.moveSpeed, player.config.jumpForce);
    }

    public override bool CheckTransitionToFalling(PlayerFSM player) {
        float xInput = Input.GetAxisRaw("Horizontal");

        if (!Input.GetButton("Jump") || xInput != 0) {
            player.TransitionToState(player.FallingState);
            return true;
        }

        return false;
    }

    public override bool CheckTransitionToWallSliding(PlayerFSM player) {
        if (!player.mechanics.IsEnabled("Wall Slide")) return false;

        if (base.CheckTransitionToWallSliding(player)) return true;

        if (player.isTouchingWall) {
            player.TransitionToState(player.WallSlidingState);
            return true;
        }

        return false;
    }
}
