using UnityEngine;

public abstract class BeeBaseState {
    public abstract void EnterState(BeeFSM bee);
    public abstract void Update(BeeFSM bee);

    #region Check Methods

    public virtual bool CheckTransitionToMoving(BeeFSM bee) {
        return false;
    }

    public virtual bool CheckTransitionToBeingHit(BeeFSM bee) {
        return false;
    }
    public virtual bool CheckTransitionToAttacking(BeeFSM bee) {
        return false;
    }

    #endregion

    #region Helper Functions
    #endregion
}