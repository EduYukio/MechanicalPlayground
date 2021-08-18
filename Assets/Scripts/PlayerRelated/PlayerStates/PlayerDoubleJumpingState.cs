using UnityEngine;

public class PlayerDoubleJumpingState : PlayerBaseState {
    public override void EnterState(PlayerFSM player) {
        Setup(player);
        PlayAnimation(player);
        PlayParticles(player);
        DoubleJumpAction(player);
    }

    public override void Update(PlayerFSM player) {
        base.ProcessMovementInput(player);

        if (base.CheckTransitionToGunBoots(player)) return;
        if (base.CheckTransitionToGrounded(player)) return;
        if (base.CheckTransitionToFalling(player)) return;
        if (base.CheckTransitionToDashing(player)) return;
        if (base.CheckTransitionToAttacking(player)) return;
        if (base.CheckTransitionToBlinking(player)) return;
        if (base.CheckTransitionToWallSliding(player)) return;
        if (base.CheckTransitionToExploding(player)) return;
    }

    private void Setup(PlayerFSM player) {
        player.canDoubleJump = false;
    }

    private void PlayAnimation(PlayerFSM player) {
        player.animator.Play("PlayerDoubleJump");
    }

    private void PlayParticles(PlayerFSM player) {
        player.jumpParticles.Play();
    }

    private void DoubleJumpAction(PlayerFSM player) {
        player.rb.velocity = new Vector2(player.rb.velocity.x, player.config.doubleJumpForce);
    }
}
