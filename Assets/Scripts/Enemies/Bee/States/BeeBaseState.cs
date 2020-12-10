using UnityEngine;

public abstract class BeeBaseState {
    public abstract void EnterState(BeeFSM bee);
    public abstract void Update(BeeFSM bee);

    #region Check Methods

    public virtual bool CheckTransitionToMoving(BeeFSM bee) {
        bee.TransitionToState(bee.MovingState);
        return true;
    }

    public virtual bool CheckTransitionToBeingHit(BeeFSM bee) {
        if (bee.isBeingHit) {
            bee.TransitionToState(bee.BeingHitState);
            return true;
        }
        return false;
    }

    public virtual bool CheckTransitionToAttacking(BeeFSM bee) {
        if (bee.attackCooldownTimer <= 0) {
            bee.TransitionToState(bee.AttackingState);
            return true;
        }
        return false;
    }

    public virtual bool CheckTransitionToDying(BeeFSM bee) {
        if (bee.currentHealth <= 0) {
            bee.TransitionToState(bee.DyingState);
            return true;
        }

        return false;
    }

    #endregion



    #region Helper Functions

    public void MoveAction(BeeFSM bee) {
        float step = bee.moveSpeed * Time.deltaTime;
        bee.transform.position = Vector2.MoveTowards(bee.transform.position, bee.targetPosition, step);

        if (Vector2.Distance(bee.transform.position, bee.targetPosition) < 0.01f) {
            bee.distanceToMove *= -1f;
            bee.targetPosition = NewTargetPosition(bee);
        }
    }

    Vector2 NewTargetPosition(BeeFSM bee) {
        if (bee.moveVertically) {
            return new Vector2(bee.transform.position.x, bee.initialCoord + bee.distanceToMove);
        }
        else {
            return new Vector2(bee.initialCoord + bee.distanceToMove, bee.transform.position.y);
        }
    }

    #endregion
}