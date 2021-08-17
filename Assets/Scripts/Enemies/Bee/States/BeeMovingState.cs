public class BeeMovingState : BeeBaseState {
    public override void EnterState(BeeFSM bee) {
    }

    public override void Update(BeeFSM bee) {
        PlayAnimationIfCan(bee);
        base.MoveAction(bee);

        if (base.CheckTransitionToAttacking(bee)) return;
    }

    private void PlayAnimationIfCan(BeeFSM bee) {
        if (Helper.IsPlayingAnimation("Attacking", bee.animator)) return;
        if (Helper.IsPlayingAnimation("Moving", bee.animator)) return;

        bee.animator.Play("Moving");
    }
}