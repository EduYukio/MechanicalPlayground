using UnityEngine;

public class PlayerWalkingState : PlayerBaseState {
    private float startWalkParticlesCooldownTime;
    private float startCoyoteDurationTime;
    private string[] waitAnimations;

    public override void EnterState(PlayerFSM player) {
        Setup(player);
    }

    public override void Update(PlayerFSM player) {
        Helper.PlayAnimationIfPossible("PlayerWalk", player.animator, waitAnimations);
        PlayParticleIfCan(player);
        ResetCoyoteTimer(player);
        base.ProcessHorizontalMoveInput(player);

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
        startWalkParticlesCooldownTime = player.config.startWalkParticlesCooldownTime;
        startCoyoteDurationTime = player.config.startCoyoteDurationTime;
        player.walkParticlesCooldownTimer = 0f;
        waitAnimations = new string[] { "PlayerWalk", "PlayerAttacking", "PlayerAttackingBoosted", "PlayerExploding", "PlayerAppear" };
    }

    private void ResetCoyoteTimer(PlayerFSM player) {
        player.coyoteTimer = startCoyoteDurationTime;
    }

    private void PlayParticleIfCan(PlayerFSM player) {
        if (player.walkParticlesCooldownTimer > 0) return;

        player.walkParticles.Play();
        player.walkParticlesCooldownTimer = startWalkParticlesCooldownTime;
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
}
