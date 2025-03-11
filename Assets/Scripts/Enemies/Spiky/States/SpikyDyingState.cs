public class SpikyDyingState : SpikyBaseState
{
    public override void EnterState(SpikyFSM spiky)
    {
        Enemy.DieAction(spiky.gameObject);
    }
}