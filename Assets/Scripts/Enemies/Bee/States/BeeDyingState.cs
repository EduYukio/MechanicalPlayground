public class BeeDyingState : BeeBaseState {
    public override void EnterState(BeeFSM bee) {
        Enemy.DieAction(bee.gameObject);
    }

    public override void Update(BeeFSM bee) {
    }
}