using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    private string[] waitAnimations;

    public override void EnterState(PlayerFSM player)
    {
        Setup(player);
        GroundedAction(player);
    }

    public override void Update(PlayerFSM player)
    {
        Helper.PlayAnimationIfPossible("PlayerIdle", player.animator, waitAnimations);

        if (base.CheckTransitionToGunBoots(player)) return;
        if (base.CheckTransitionToFalling(player)) return;
        if (base.CheckTransitionToJumping(player)) return;
        if (base.CheckTransitionToWalking(player)) return;
        if (base.CheckTransitionToDashing(player)) return;
        if (base.CheckTransitionToAttacking(player)) return;
        if (base.CheckTransitionToBlinking(player)) return;
        if (base.CheckTransitionToExploding(player)) return;
    }

    private void Setup(PlayerFSM player)
    {
        player.canDoubleJump = true;
        player.canDash = true;
        waitAnimations = new string[] { "PlayerIdle", "PlayerAttacking", "PlayerAttackingBoosted", "PlayerAppear", "PlayerDisappear", "PlayerExploding" };
    }

    private void GroundedAction(PlayerFSM player)
    {
        player.rb.velocity = Vector2.zero;
    }
}
