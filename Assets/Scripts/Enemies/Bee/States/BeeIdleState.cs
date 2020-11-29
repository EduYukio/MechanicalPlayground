using UnityEngine;

public class BeeIdleState : BeeBaseState {
    public override void EnterState(BeeFSM bee) {
        bee.animator.Play("BeeIdle");
    }

    public override void Update(BeeFSM bee) {

    }
}