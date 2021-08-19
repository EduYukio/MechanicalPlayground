using UnityEngine;

public class PlayerWallSlidingState : PlayerBaseState {
    private float stickyTimer, startStickyTime;
    private float slideSpeed, maxSpeed;

    public override void EnterState(PlayerFSM player) {
        Setup(player);
        PlayAnimation(player);
        PlayAudio();
    }

    public override void FixedUpdate(PlayerFSM player) {
        if (base.CheckTransitionToGunBoots(player)) return;
        if (CheckTransitionToGrounded(player)) return;
        if (base.CheckTransitionToWallJumping(player)) return;
        if (base.CheckTransitionToDashing(player)) return;
        if (base.CheckTransitionToBlinking(player)) return;
        if (base.CheckTransitionToExploding(player)) return;
        if (CheckDettachmentFromWall(player)) return;

        if (stickyTimer > 0) {
            WallSlideAction(player);
            ProcessStickyTimer(player);
        }

        if (CheckTransitionToFalling(player)) return;
    }

    private void Setup(PlayerFSM player) {
        player.canDoubleJump = true;
        player.canDash = true;
        startStickyTime = player.config.startStickyTime;
        stickyTimer = startStickyTime;
        maxSpeed = float.MaxValue;
        slideSpeed = -player.config.wallSlidingSpeed;
        base.SetLookingDirectionOppositeOfWall(player);
    }

    private void PlayAnimation(PlayerFSM player) {
        player.animator.Play("PlayerWallSlide", -1, 0f);
    }

    private void PlayAudio() {
        Manager.audio.Play("Wall Slide Begin");
    }

    private void WallSlideAction(PlayerFSM player) {
        float yVelocity = Mathf.Clamp(player.rb.velocity.y, slideSpeed, maxSpeed);
        player.rb.velocity = new Vector2(player.rb.velocity.x, yVelocity);
    }

    private void ProcessStickyTimer(PlayerFSM player) {
        if (IsMovingAwayFromWall(player)) {
            stickyTimer -= Time.deltaTime;
        }
        else {
            stickyTimer = startStickyTime;
        }
    }

    private bool IsMovingAwayFromWall(PlayerFSM player) {
        float xInput = Input.GetAxisRaw("Horizontal");
        if (xInput == 0) return false;
        if (xInput < 0 && player.isTouchingLeftWall) return false;
        if (xInput > 0 && player.isTouchingRightWall) return false;

        return true;
    }

    public bool CheckDettachmentFromWall(PlayerFSM player) {
        if (!player.mechanics.IsEnabled("Wall Jump")) {
            if (Input.GetButtonDown("Jump")) {
                player.TransitionToState(player.FallingState);
                return true;
            }
        }
        return false;
    }

    public override bool CheckTransitionToFalling(PlayerFSM player) {
        if (stickyTimer <= 0 || !player.isTouchingWall) {
            player.TransitionToState(player.FallingState);
            return true;
        }
        return false;
    }

    public override bool CheckTransitionToGrounded(PlayerFSM player) {
        if (player.rb.velocity.y > 0) return false;
        return base.CheckTransitionToGrounded(player);
    }
}
