using UnityEngine;

public class PlayerWalkingState : PlayerBaseState {
    private float walkParticlesStartCooldown = 0.5f;

    public override void EnterState(PlayerFSM player) {
        Setup(player);
    }

    public override void Update(PlayerFSM player) {
        PlayAnimationIfCan(player);
        PlayParticleIfCan(player);
        ResetCoyoteTimer(player);
        base.ProcessMovementInput(player);

        if (base.CheckTransitionToGunBoots(player)) return;
        if (CheckTransitionToGrounded(player)) return;
        if (base.CheckTransitionToFalling(player)) return;
        if (base.CheckTransitionToJumping(player)) return;
        if (base.CheckTransitionToDashing(player)) return;
        if (base.CheckTransitionToAttacking(player)) return;
        if (base.CheckTransitionToBlinking(player)) return;
        if (base.CheckTransitionToExploding(player)) return;
    }

    private void Setup(PlayerFSM player) {
        player.walkParticlesCooldownTimer = 0f;
    }

    private void ResetCoyoteTimer(PlayerFSM player) {
        player.coyoteTimer = player.config.startCoyoteDurationTime;
    }

    public override bool CheckTransitionToGrounded(PlayerFSM player) {
        if (!player.isGrounded) return false;

        float xInput = Input.GetAxisRaw("Horizontal");
        if (xInput == 0) {
            player.TransitionToState(player.GroundedState);
            return true;
        }
        return false;
    }

    private void PlayAnimationIfCan(PlayerFSM player) {
        if (Helper.IsPlayingAnimation("PlayerWalk", player.animator)) return;
        if (Helper.IsPlayingAnimation("PlayerAttacking", player.animator)) return;
        if (Helper.IsPlayingAnimation("PlayerAttackingBoosted", player.animator)) return;
        if (Helper.IsPlayingAnimation("PlayerExploding", player.animator)) return;

        player.animator.Play("PlayerWalk");
    }

    private void PlayParticleIfCan(PlayerFSM player) {
        if (player.walkParticlesCooldownTimer > 0) return;

        player.walkParticles.Play();
        player.walkParticlesCooldownTimer = walkParticlesStartCooldown;
    }
}
