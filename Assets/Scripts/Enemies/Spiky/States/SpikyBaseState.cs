public abstract class SpikyBaseState
{
    public abstract void EnterState(SpikyFSM spiky);
    public virtual void Update(SpikyFSM spiky) { }
    public virtual void FixedUpdate(SpikyFSM spiky) { }

    public virtual bool CheckTransitionToIdle(SpikyFSM spiky)
    {
        spiky.TransitionToState(spiky.IdleState);
        return true;
    }

    public virtual bool CheckTransitionToAttacking(SpikyFSM spiky)
    {
        if (spiky.attackCooldownTimer <= 0)
        {
            spiky.TransitionToState(spiky.AttackingState);
            return true;
        }
        return false;
    }

    public virtual bool CheckTransitionToDying(SpikyFSM spiky)
    {
        if (spiky.currentHealth <= 0)
        {
            spiky.TransitionToState(spiky.DyingState);
            return true;
        }

        return false;
    }
}