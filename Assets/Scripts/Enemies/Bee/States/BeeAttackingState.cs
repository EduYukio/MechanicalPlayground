using UnityEngine;

public class BeeAttackingState : BeeBaseState {
    public override void EnterState(BeeFSM bee) {
        bee.animator.Play("BeeAttacking");
    }

    public override void Update(BeeFSM bee) {

    }
}