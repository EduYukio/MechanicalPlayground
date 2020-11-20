using UnityEngine;

public abstract class PlayerBaseState {
    public abstract void EnterState(PlayerFSM player);
    public abstract void Update(PlayerFSM player);

    public void ProcessMovementInput(PlayerFSM player) {
        float xInput = Input.GetAxisRaw("Horizontal");
        int direction = 0;
        if (xInput > 0) {
            direction = 1;
            player.lastDirection = direction;
        }
        else if (xInput < 0) {
            direction = -1;
            player.lastDirection = direction;
        }

        player.rb.velocity = new Vector2(direction * player.config.moveSpeed, player.rb.velocity.y);
    }

    public virtual void CheckTransitionToDashing(PlayerFSM player) {
        if (!player.mechanics.dash) return;
        if (!player.canDash) return;
        if (player.dashCooldownTimer > 0) return;

        // GamePad || Keyboard
        if (Input.GetAxisRaw("Dash") > 0 || Input.GetButtonDown("Dash")) {
            player.TransitionToState(player.DashingState);
        }
    }

    public virtual void CheckTransitionToGrounded(PlayerFSM player) {
        if (player.isGrounded) {
            player.TransitionToState(player.GroundedState);
        }
    }

    public virtual void CheckTransitionToFalling(PlayerFSM player) {
        if (player.isGrounded) return;

        bool playerIsFalling = player.rb.velocity.y <= 0;
        bool playerStoppedJumping = player.rb.velocity.y > 0 && !Input.GetButton("Jump");

        if (playerIsFalling || playerStoppedJumping) {
            player.TransitionToState(player.FallingState);
        }
    }

    public virtual void CheckTransitionToDoubleJump(PlayerFSM player) {
        if (!player.mechanics.doubleJump) return;
        if (!player.canDoubleJump) return;

        if (Input.GetButtonDown("Jump")) {
            player.TransitionToState(player.DoubleJumpingState);
        }
    }

    public virtual void CheckTransitionToJumping(PlayerFSM player) {
        if (!player.mechanics.jump) return;

        if (Input.GetButtonDown("Jump")) {
            player.TransitionToState(player.JumpingState);
        }
    }

    public virtual void CheckTransitionToWalking(PlayerFSM player) {
        if (!player.mechanics.walk) return;

        float xInput = Input.GetAxisRaw("Horizontal");
        if (xInput != 0) {
            player.TransitionToState(player.WalkingState);
        }
    }

    public virtual void CheckTransitionToWallSliding(PlayerFSM player) {
        if (!player.mechanics.wallSlide) return;

        float xInput = Input.GetAxisRaw("Horizontal");
        bool pressingAgainstLeftWall = player.isTouchingLeftWall && xInput < 0;
        bool pressingAgainstRightWall = player.isTouchingRightWall && xInput > 0;

        if (pressingAgainstLeftWall || pressingAgainstRightWall) {
            player.TransitionToState(player.WallSlidingState);
        }
    }
}
