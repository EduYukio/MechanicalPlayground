using UnityEngine;

public class BeeHitState : BeeBaseState {
    public override void EnterState(BeeFSM bee) {
        bee.animator.Play("BeeHit");
    }

    public override void Update(BeeFSM bee) {

    }
}