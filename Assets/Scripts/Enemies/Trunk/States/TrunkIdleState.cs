using System;
using UnityEngine;

public class TrunkIdleState : TrunkBaseState {
    float idleTimer;

    public override void EnterState(TrunkFSM trunk) {
        Debug.Log("Entered Idle");
        trunk.animator.Play("Idle");
        Setup(trunk);
        IdleAction(trunk);
    }

    public override void Update(TrunkFSM trunk) {
        // if (base.CheckTransitionToBeingHit(trunk)) return;
        // if (base.CheckTransitionToAttacking(trunk)) return;

        if (idleTimer >= 0) {
            idleTimer -= Time.deltaTime;
            return;
        }

        if (CheckTransitionToMoving(trunk)) return;
    }

    void Setup(TrunkFSM trunk) {
        idleTimer = Helper.GetAnimationDuration("Idle", trunk.animator);
    }

    void IdleAction(TrunkFSM trunk) {
        trunk.rb.velocity = Vector2.zero;
    }

    private void InvertDirectionIfNeeded(TrunkFSM trunk) {
        if (trunk.needToTurn) {
            if (trunk.transform.eulerAngles.y == 0) {
                trunk.transform.eulerAngles = new Vector3(0f, 180f, 0f);
            }
            else if (trunk.transform.eulerAngles.y == 180) {
                trunk.transform.eulerAngles = new Vector3(0f, 0f, 0f);
            }
        }
        trunk.needToTurn = false;
    }

    public override bool CheckTransitionToMoving(TrunkFSM trunk) {
        InvertDirectionIfNeeded(trunk);

        trunk.TransitionToState(trunk.MovingState);
        return true;
    }
}