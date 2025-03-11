using UnityEngine;

public class TrunkIdleState : TrunkBaseState
{
    private float idleTimer;
    private string[] waitAnimations;

    public override void EnterState(TrunkFSM trunk)
    {
        Setup(trunk);
        IdleAction(trunk);
    }

    public override void FixedUpdate(TrunkFSM trunk)
    {
        Helper.PlayAnimationIfPossible("Idle", trunk.animator, waitAnimations);
        if (base.CheckTransitionToAttacking(trunk)) return;

        if (idleTimer >= 0)
        {
            idleTimer -= Time.deltaTime;
            return;
        }

        if (base.CheckTransitionToMoving(trunk)) return;
    }

    private void Setup(TrunkFSM trunk)
    {
        idleTimer = Helper.GetAnimationDuration("Idle", trunk.animator);
        waitAnimations = new string[] { "Attacking", "Idle" };
    }

    private void IdleAction(TrunkFSM trunk)
    {
        trunk.rb.velocity = Vector2.zero;
    }
}