using UnityEngine;

public class PlayerDashingState : PlayerBaseState
{
    private float dashTimer;
    private float originalGravity;
    private bool isEthereal;

    public override void EnterState(PlayerFSM player)
    {
        Setup(player);
        PlayAnimation(player);
        PlayAudio();
        DashAction(player);
    }

    public override void FixedUpdate(PlayerFSM player)
    {
        CheckForAirJumpInputBuffer(player);

        if (dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;
            return;
        }

        StopDashing(player);
        if (base.CheckTransitionToGunBoots(player)) return;
        if (base.CheckTransitionToGrounded(player)) return;
        if (base.CheckTransitionToDoubleJumping(player)) return;
        if (CheckTransitionToWallSliding(player)) return;
        if (CheckTransitionToFalling(player)) return;
    }

    private void Setup(PlayerFSM player)
    {
        dashTimer = player.config.startDashDurationTime;
        isEthereal = player.mechanics.IsEnabled("Ethereal Dash");
        originalGravity = player.rb.gravityScale;
        player.rb.gravityScale = 0f;
        player.hasResetDashTrigger = false;
    }

    private void PlayAnimation(PlayerFSM player)
    {
        player.animator.Play("PlayerDash");
    }

    private void PlayAudio()
    {
        Manager.audio.Play("Slash2");
    }

    private void DashAction(PlayerFSM player)
    {
        player.rb.velocity = new Vector2(player.lookingDirection * player.config.dashSpeed, 0f);

        if (isEthereal)
        {
            player.spriteRenderer.color = new Color(1, 1, 1, player.config.etherealTransparency);
            player.gameObject.layer = LayerMask.NameToLayer("PlayerEthereal");
        }
    }

    private void StopDashing(PlayerFSM player)
    {
        player.rb.velocity = Vector2.zero;

        player.rb.gravityScale = originalGravity;
        player.canDash = false;
        player.dashCooldownTimer = player.config.startDashCooldownTime;

        if (isEthereal)
        {
            player.spriteRenderer.color = new Color(1, 1, 1, 1);
            player.gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }

    private void CheckForAirJumpInputBuffer(PlayerFSM player)
    {
        if (Input.GetButtonDown("Jump"))
        {
            player.airJumpInputBufferTimer = player.config.startAirJumpInputBufferTime;
        }
    }

    public override bool CheckTransitionToWallSliding(PlayerFSM player)
    {
        if (!player.mechanics.IsEnabled("Wall Slide")) return false;

        if (player.isTouchingWall)
        {
            player.TransitionToState(player.WallSlidingState);
            return true;
        }
        return false;
    }

    public override bool CheckTransitionToFalling(PlayerFSM player)
    {
        if (!player.isGrounded)
        {
            player.TransitionToState(player.FallingState);
            return true;
        }
        return false;
    }
}
