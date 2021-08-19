using UnityEngine;

public abstract class PlayerBaseState {
    public abstract void EnterState(PlayerFSM player);
    public virtual void Update(PlayerFSM player) { }
    public virtual void FixedUpdate(PlayerFSM player) { }

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
        if (!player.mechanics.IsEnabled("Double Jump")) return false;
        if (!player.canDoubleJump) return false;

        if (Input.GetButtonDown("Jump") || player.airJumpInputBufferTimer > 0) {
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
        if (!player.mechanics.IsEnabled("Wall Slide")) return false;

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
        if (!player.mechanics.IsEnabled("Wall Jump")) return false;
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

    public virtual bool CheckTransitionToExploding(PlayerFSM player) {
        if (!player.mechanics.IsEnabled("Explosion")) return false;
        if (player.explosionCooldownTimer > 0) return false;

        if (Input.GetButtonDown("Explosion")) {
            player.TransitionToState(player.ExplodingState);
            return true;
        }

        return false;
    }

    public virtual bool CheckTransitionToGunBoots(PlayerFSM player) {
        if (!player.mechanics.IsEnabled("Gun Boots")) return false;

        if (Input.GetAxisRaw("Gun Boots") > 0 || Input.GetButton("Gun Boots")) {
            player.TransitionToState(player.GunBootsState);
            return true;
        }

        return false;
    }



    public void ProcessHorizontalMoveInput(PlayerFSM player) {
        float xInput = Input.GetAxisRaw("Horizontal");
        int direction = GetRawDirection(xInput);
        if (direction != 0) {
            player.lastDirection = direction;
        }

        player.rb.velocity = new Vector2(direction * player.moveSpeed, player.rb.velocity.y);
    }

    // For the GamePad analog sticks
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

    public Vector3 GetFourDirectionalInput(PlayerFSM player, float xInput, float yInput) {
        Vector3 direction;
        if (xInput == 0 && yInput == 0) {
            direction = new Vector3(player.lastDirection, 0f, 0f);
        }
        else if (Mathf.Abs(xInput) > Mathf.Abs(yInput)) {
            int xDirection = GetRawDirection(xInput);
            direction = new Vector3(xDirection, 0f, 0f);
        }
        else {
            int yDirection = GetRawDirection(yInput);
            direction = new Vector3(0f, yDirection, 0f);
        }
        return direction;
    }

    public void SetLookingDirectionOppositeOfWall(PlayerFSM player) {
        if (player.isTouchingLeftWall) {
            player.lastDirection = 1;
        }
        else if (player.isTouchingRightWall) {
            player.lastDirection = -1;
        }
    }
}
