public class BeeMovingState : BeeBaseState
{
    private string[] waitAnimations;

    public override void EnterState(BeeFSM bee)
    {
        Setup();
    }

    public override void Update(BeeFSM bee)
    {
        Helper.PlayAnimationIfPossible("Moving", bee.animator, waitAnimations);
        base.MoveAction(bee);

        if (base.CheckTransitionToAttacking(bee)) return;
    }

    private void Setup()
    {
        waitAnimations = new string[] { "Attacking", "Moving" };
    }
}