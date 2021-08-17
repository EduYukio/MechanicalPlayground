public class SpikyDyingState : SpikyBaseState {
    public override void EnterState(SpikyFSM spiky) {
        Enemy.DieAction(spiky.gameObject);
    }

    public override void Update(SpikyFSM spiky) {
    }
}