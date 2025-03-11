using UnityEngine;

public class PlayerPogoingState : PlayerBaseState
{
    private string[] waitAnimations;

    public override void EnterState(PlayerFSM player)
    {
        Setup(player);
        PogoAction(player);
    }

    public override void Update(PlayerFSM player)
    {
        Helper.PlayAnimationIfPossible("PlayerJump", player.animator, waitAnimations);
        base.ProcessHorizontalMoveInput(player);

        if (base.CheckTransitionToGrounded(player)) return;
        if (base.CheckTransitionToGunBoots(player)) return;
        if (base.CheckTransitionToWallSliding(player)) return;
        if (CheckTransitionToFalling(player)) return;
        if (base.CheckTransitionToDashing(player)) return;
        if (base.CheckTransitionToDoubleJumping(player)) return;
        if (base.CheckTransitionToAttacking(player)) return;
        if (base.CheckTransitionToBlinking(player)) return;
        if (base.CheckTransitionToExploding(player)) return;
    }

    private void Setup(PlayerFSM player)
    {
        player.canDoubleJump = true;
        player.canDash = true;
        waitAnimations = new string[] { "PlayerJump", "PlayerAttacking", "PlayerAttackingBoosted" };
    }

    private void PogoAction(PlayerFSM player)
    {
        player.rb.velocity = new Vector2(player.rb.velocity.x, player.config.pogoForce);
    }

    public override bool CheckTransitionToFalling(PlayerFSM player)
    {
        if (player.isGrounded) return false;

        if (player.rb.velocity.y <= 0)
        {
            player.TransitionToState(player.FallingState);
            return true;
        }

        return false;
    }
}
