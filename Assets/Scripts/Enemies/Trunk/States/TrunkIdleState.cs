using System;
using UnityEngine;

public class TrunkIdleState : TrunkBaseState {
    float idleTimer;

    public override void EnterState(TrunkFSM trunk) {
        trunk.animator.Play("Idle");
        Setup(trunk);
        IdleAction(trunk);
    }

    public override void Update(TrunkFSM trunk) {
        if (base.CheckTransitionToBeingHit(trunk)) return;
        if (base.CheckTransitionToAttacking(trunk)) return;

        if (idleTimer >= 0) {
            idleTimer -= Time.deltaTime;
            return;
        }

        if (base.CheckTransitionToMoving(trunk)) return;
    }

    void Setup(TrunkFSM trunk) {
        idleTimer = Helper.GetAnimationDuration("Idle", trunk.animator);
    }

    void IdleAction(TrunkFSM trunk) {
        trunk.rb.velocity = Vector2.zero;
    }
}