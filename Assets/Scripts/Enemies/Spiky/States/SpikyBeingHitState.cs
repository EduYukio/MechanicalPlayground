using UnityEngine;

public class SpikyBeingHitState : SpikyBaseState {
    float hitTimer;

    public override void EnterState(SpikyFSM spiky) {
        spiky.animator.Play("BeingHit");
        Setup(spiky);
        BeingHitAction(spiky);
    }

    public override void Update(SpikyFSM spiky) {
        if (base.CheckTransitionToDying(spiky)) return;

        if (hitTimer >= 0) {
            hitTimer -= Time.deltaTime;
            return;
        }

        if (base.CheckTransitionToIdle(spiky)) return;
    }

    void Setup(SpikyFSM spiky) {
        hitTimer = Helper.GetAnimationDuration("BeingHit", spiky.animator);
        spiky.isBeingHit = false;
    }

    void BeingHitAction(SpikyFSM spiky) {
        spiky.rb.velocity = Vector2.zero;
    }
}