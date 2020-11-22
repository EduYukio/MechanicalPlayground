using UnityEngine;

public class PlayerWallJumpingState : PlayerBaseState {
    private float xVelocityTimer = 10f;

    public override void EnterState(PlayerFSM player) {
        player.animator.Play("PlayerJump");
        player.canDoubleJump = true;
        player.canDash = true;
        xVelocityTimer = player.config.startWallJumpDurationTime;
        base.SetPlayerSpriteOppositeOfWall(player);
        ApplyXVelocity(player);
        JumpAction(player);
    }

    public override void Update(PlayerFSM player) {
        base.CheckTransitionToDashing(player);
        // Obligatory x movement
        if (xVelocityTimer > 0) {
            xVelocityTimer -= Time.deltaTime;
            return;
        }

        CheckTransitionToWallSliding(player);
        CheckTransitionToFalling(player);
        base.CheckTransitionToGrounded(player);
        base.CheckTransitionToAttacking(player);
    }

    public override void CheckTransitionToFalling(PlayerFSM player) {
        float xInput = Input.GetAxisRaw("Horizontal");
        if (!Input.GetButton("Jump") || xInput != 0) {
            player.TransitionToState(player.FallingState);
        }
    }

    public override void CheckTransitionToWallSliding(PlayerFSM player) {
        base.CheckTransitionToWallSliding(player);

        if (player.isTouchingWall) {
            player.TransitionToState(player.WallSlidingState);
        }
    }

    void ApplyXVelocity(PlayerFSM player) {
        player.rb.velocity = new Vector2(player.config.moveSpeed * player.lastDirection, player.rb.velocity.y);
    }

    void JumpAction(PlayerFSM player) {
        player.rb.velocity = new Vector2(player.rb.velocity.x, player.config.jumpForce);
    }
}
