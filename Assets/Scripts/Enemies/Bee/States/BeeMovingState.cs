using UnityEngine;

public class BeeMovingState : BeeBaseState {
    public override void EnterState(BeeFSM bee) {
        bee.animator.Play("Moving");
    }

    public override void Update(BeeFSM bee) {
        base.MoveAction(bee);

        if (base.CheckTransitionToBeingHit(bee)) return;
        if (base.CheckTransitionToAttacking(bee)) return;
    }
}