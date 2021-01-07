using UnityEngine;

public class TrunkBeingHitState : TrunkBaseState {
    float hitTimer;

    public override void EnterState(TrunkFSM trunk) {
        trunk.animator.Play("BeingHit", -1, 0f);
        Manager.audio.Play("EnemyHit");
        Setup(trunk);
        BeingHitAction(trunk);
    }

    public override void Update(TrunkFSM trunk) {
        if (base.CheckTransitionToDying(trunk)) return;

        if (hitTimer >= 0) {
            hitTimer -= Time.deltaTime;
            return;
        }

        if (base.CheckTransitionToAttacking(trunk)) return;
        if (base.CheckTransitionToMoving(trunk)) return;
    }

    void Setup(TrunkFSM trunk) {
        hitTimer = Helper.GetAnimationDuration("BeingHit", trunk.animator);
    }

    void BeingHitAction(TrunkFSM trunk) {
        trunk.rb.velocity = Vector2.zero;
    }
}