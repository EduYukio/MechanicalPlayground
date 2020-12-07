using UnityEngine;

public abstract class TrunkBaseState {
    public abstract void EnterState(TrunkFSM trunk);
    public abstract void Update(TrunkFSM trunk);

    #region Check Methods

    public virtual bool CheckTransitionToIdle(TrunkFSM trunk) {
        trunk.TransitionToState(trunk.IdleState);
        return true;
    }

    public virtual bool CheckTransitionToMoving(TrunkFSM trunk) {
        trunk.TransitionToState(trunk.MovingState);
        return true;
    }

    public virtual bool CheckTransitionToBeingHit(TrunkFSM trunk) {
        if (trunk.isBeingHit) {
            trunk.TransitionToState(trunk.BeingHitState);
            return true;
        }
        return false;
    }

    public virtual bool CheckTransitionToAttacking(TrunkFSM trunk) {
        if (trunk.attackCooldownTimer <= 0) {
            trunk.TransitionToState(trunk.AttackingState);
            return true;
        }
        return false;
    }

    public virtual bool CheckTransitionToDying(TrunkFSM trunk) {
        if (trunk.currentHealth <= 0) {
            trunk.TransitionToState(trunk.DyingState);
            return true;
        }

        return false;
    }

    #endregion

    #region Helper Functions

    public void MoveAction(TrunkFSM trunk) {
        trunk.transform.Translate(Vector2.left * trunk.moveSpeed * Time.deltaTime);
    }

    #endregion
}