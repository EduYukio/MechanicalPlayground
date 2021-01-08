using UnityEngine;

public class PlayerWallJumpingState : PlayerBaseState {
    private float xVelocityTimer = 10f;

    public override void EnterState(PlayerFSM player) {
        player.animator.Play("PlayerJump");
        Setup(player);
        ApplyXVelocity(player);
        JumpAction(player);
    }

    public override void Update(PlayerFSM player) {
        if (base.CheckTransitionToDashing(player)) return;
        if (base.CheckTransitionToBlinking(player)) return;
        if (base.CheckTransitionToExploding(player)) return;
        if (base.CheckTransitionToGunBoots(player)) return;

        // Obligatory x movement
        if (xVelocityTimer > 0) {
            xVelocityTimer -= Time.deltaTime;
            return;
        }

        if (CheckTransitionToWallSliding(player)) return;
        if (CheckTransitionToFalling(player)) return;
        if (base.CheckTransitionToGrounded(player)) return;
        if (base.CheckTransitionToAttacking(player)) return;
    }

    void Setup(PlayerFSM player) {
        player.jumpParticles.Play();
        player.canDoubleJump = true;
        player.canDash = true;
        xVelocityTimer = player.config.startWallJumpDurationTime;
        base.SetPlayerSpriteOppositeOfWall(player);
    }

    void JumpAction(PlayerFSM player) {
        player.rb.velocity = new Vector2(player.rb.velocity.x, player.config.jumpForce);
    }

    void ApplyXVelocity(PlayerFSM player) {
        player.rb.velocity = new Vector2(player.lastDirection * player.moveSpeed, player.rb.velocity.y);
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
