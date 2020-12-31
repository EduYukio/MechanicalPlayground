using UnityEngine;

public class BeeBeingHitState : BeeBaseState {
    float hitTimer;

    public override void EnterState(BeeFSM bee) {
        bee.animator.Play("BeingHit", -1, 0f);
        Setup(bee);
        BeingHitAction(bee);
    }

    public override void Update(BeeFSM bee) {
        if (base.CheckTransitionToDying(bee)) return;

        if (hitTimer >= 0) {
            hitTimer -= Time.deltaTime;
            return;
        }

        if (base.CheckTransitionToMoving(bee)) return;
    }

    void Setup(BeeFSM bee) {
        hitTimer = Helper.GetAnimationDuration("BeingHit", bee.animator);
    }

    void BeingHitAction(BeeFSM bee) {
        bee.rb.velocity = Vector2.zero;
    }
}