using UnityEngine;

public class PlayerWallSlidingState : PlayerBaseState {
    private float stickyTimer;

    public override void EnterState(PlayerFSM player) {
        player.animator.Play("PlayerWallSlide");
        player.canDoubleJump = true;
        player.canDash = true;
        stickyTimer = player.config.startStickyTime;
        base.SetPlayerSpriteOppositeOfWall(player);
    }

    public override void Update(PlayerFSM player) {
        if (base.CheckTransitionToGrounded(player)) return;
        if (base.CheckTransitionToWallJumping(player)) return;
        if (base.CheckTransitionToDashing(player)) return;

        if (stickyTimer > 0) {
            WallSlideAction(player);
            ProcessStickyTimer(player);
        }

        if (CheckTransitionToFalling(player)) return;
    }

    public override bool CheckTransitionToFalling(PlayerFSM player) {
        if (stickyTimer <= 0 || !player.isTouchingWall) {
            player.TransitionToState(player.FallingState);
            return true;
        }
        return false;
    }

    void WallSlideAction(PlayerFSM player) {
        float yVelocity = Mathf.Clamp(player.rb.velocity.y, -player.config.wallSlidingSpeed, float.MaxValue);
        player.rb.velocity = new Vector2(player.rb.velocity.x, yVelocity);
    }

    void ProcessStickyTimer(PlayerFSM player) {
        if (IsMovingAwayFromWall(player)) {
            stickyTimer -= Time.deltaTime;
        }
        else {
            stickyTimer = player.config.startStickyTime;
        }
    }

    bool IsMovingAwayFromWall(PlayerFSM player) {
        float xInput = Input.GetAxisRaw("Horizontal");
        if (xInput == 0) return false;
        if (xInput < 0 && player.isTouchingLeftWall) return false;
        if (xInput > 0 && player.isTouchingRightWall) return false;

        return true;
    }
}
