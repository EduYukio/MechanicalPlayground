using UnityEngine;

public class SpikyIdleState : SpikyBaseState
{
    public override void EnterState(SpikyFSM spiky)
    {
        PlayAnimation(spiky);
        IdleAction(spiky);
    }

    public override void Update(SpikyFSM spiky)
    {
        if (base.CheckTransitionToAttacking(spiky)) return;
    }

    private void PlayAnimation(SpikyFSM spiky)
    {
        spiky.animator.Play("Idle");
    }

    private void IdleAction(SpikyFSM spiky)
    {
        spiky.rb.velocity = Vector2.zero;
    }
}