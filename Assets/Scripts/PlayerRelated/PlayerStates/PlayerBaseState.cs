using UnityEngine;

public abstract class PlayerBaseState {
    public abstract void EnterState(PlayerFSM player);
    public abstract void Update(PlayerFSM player);


    #region CheckTransitionFunctions

    public virtual bool CheckTransitionToDashing(PlayerFSM player) {
        if (!player.mechanics.IsEnabled("Dash")) return false;
        if (!player.canDash) return false;
        if (!player.hasResetDashTrigger) return false;
        if (player.dashCooldownTimer > 0) return false;

        // GamePad || Keyboard
        if (Input.GetAxisRaw("Dash") > 0 || Input.GetButtonDown("Dash")) {
            player.TransitionToState(player.DashingState);
            return true;
        }

        return false;
    }

    public virtual bool CheckTransitionToGrounded(PlayerFSM player) {
        if (player.isGrounded) {
            player.TransitionToState(player.GroundedState);
            return true;
        }

        return false;
    }

    public virtual bool CheckTransitionToFalling(PlayerFSM player) {
        if (player.isGrounded) return false;

        bool playerIsFalling = player.rb.velocity.y <= 0;
        bool playerStoppedJumping = player.rb.velocity.y > 0 && !Input.GetButton("Jump");

        if (playerIsFalling || playerStoppedJumping) {
            player.TransitionToState(player.FallingState);
            return true;
        }

        return false;
    }

    public virtual bool CheckTransitionToJumping(PlayerFSM player) {
        if (!player.mechanics.IsEnabled("Jump")) return false;

        if (Input.GetButtonDown("Jump") || player.bunnyHopTimer > 0) {
            player.TransitionToState(player.JumpingState);
            return true;
        }

        return false;
    }

    public virtual bool CheckTransitionToDoubleJumping(PlayerFSM player) {
        if (!player.mechanics.IsEnabled("DoubleJump")) return false;
        if (!player.canDoubleJump) return false;
        if (player.isTouchingWall) return false;

        if (Input.GetButtonDown("Jump") || player.airJumpInputBuffer > 0) {
            player.TransitionToState(player.DoubleJumpingState);
            return true;
        }

        return false;
    }

    public virtual bool CheckTransitionToWalking(PlayerFSM player) {
        if (!player.mechanics.IsEnabled("Walk")) return false;

        float xInput = Input.GetAxisRaw("Horizontal");
        if (xInput != 0) {
            player.TransitionToState(player.WalkingState);
            return true;
        }

        return false;
    }

    public virtual bool CheckTransitionToWallSliding(PlayerFSM player) {
        if (!player.mechanics.IsEnabled("WallSlide")) return false;

        float xInput = Input.GetAxisRaw("Horizontal");
        bool pressingAgainstLeftWall = player.isTouchingLeftWall && xInput < 0;
        bool pressingAgainstRightWall = player.isTouchingRightWall && xInput > 0;

        if (pressingAgainstLeftWall || pressingAgainstRightWall) {
            player.TransitionToState(player.WallSlidingState);
            return true;
        }

        return false;
    }

    public virtual bool CheckTransitionToWallJumping(PlayerFSM player) {
        if (!player.mechanics.IsEnabled("WallJump")) return false;
        if (!player.isTouchingWall) return false;

        if (Input.GetButtonDown("Jump")) {
            player.TransitionToState(player.WallJumpingState);
            return true;
        }

        return false;
    }

    public virtual bool CheckTransitionToAttacking(PlayerFSM player) {
        if (!player.mechanics.IsEnabled("Attack")) return false;
        if (player.attackCooldownTimer > 0) return false;

        if (Input.GetButtonDown("Attack")) {
            player.TransitionToState(player.AttackingState);
            return true;
        }

        return false;
    }

    public virtual bool CheckTransitionToBlinking(PlayerFSM player) {
        if (!player.mechanics.IsEnabled("Blink")) return false;
        if (player.blinkCooldownTimer > 0) return false;

        if (Input.GetButtonDown("Blink")) {
            player.TransitionToState(player.BlinkingState);
            return true;
        }

        return false;
    }

    #endregion



    #region HelperFunctions

    public void ProcessMovementInput(PlayerFSM player) {
        float xInput = Input.GetAxisRaw("Horizontal");
        int direction = GetRawDirection(xInput);
        if (direction != 0) {
            player.lastDirection = direction;
        }

        player.rb.velocity = new Vector2(direction * player.moveSpeed, player.rb.velocity.y);
    }

    public int GetRawDirection(float input) {
        int direction = 0;
        if (input > 0) {
            direction = 1;
        }
        else if (input < 0) {
            direction = -1;
        }

        return direction;
    }

    public void SetPlayerSpriteOppositeOfWall(PlayerFSM player) {
        if (player.isTouchingLeftWall) {
            player.lastDirection = 1;
        }
        else if (player.isTouchingRightWall) {
            player.lastDirection = -1;
        }
    }

    public bool IsPlayingAnimation(string stateName, PlayerFSM player) {
        return player.animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }

    #endregion
}
