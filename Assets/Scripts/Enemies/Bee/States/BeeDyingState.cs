public class BeeDyingState : BeeBaseState
{
    public override void EnterState(BeeFSM bee)
    {
        Enemy.DieAction(bee.gameObject);
    }
}