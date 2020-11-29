using UnityEngine;

public class BeeBeingHitState : BeeBaseState {
    public override void EnterState(BeeFSM bee) {
        bee.animator.Play("BeeBeingHit");
    }

    public override void Update(BeeFSM bee) {

    }
}