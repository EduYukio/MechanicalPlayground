using UnityEngine;

public class SpikyIdleState : SpikyBaseState {
    public override void EnterState(SpikyFSM spiky) {
        spiky.animator.Play("Idle");
        IdleAction(spiky);
    }

    public override void Update(SpikyFSM spiky) {
        if (base.CheckTransitionToAttacking(spiky)) return;
    }

    private void IdleAction(SpikyFSM spiky) {
        spiky.rb.velocity = Vector2.zero;
    }
}