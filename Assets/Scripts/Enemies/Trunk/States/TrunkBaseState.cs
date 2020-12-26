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
        InvertDirectionIfNeeded(trunk);

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
        if (trunk.attackCooldownTimer <= 0 && PlayerIsOnSight(trunk)) {
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

    public void InvertDirectionIfNeeded(TrunkFSM trunk) {
        if (trunk.needToTurn) {
            if (trunk.transform.eulerAngles.y == 0) {
                trunk.transform.eulerAngles = new Vector3(0f, 180f, 0f);
            }
            else if (trunk.transform.eulerAngles.y == 180) {
                trunk.transform.eulerAngles = new Vector3(0f, 0f, 0f);
            }
        }
        trunk.needToTurn = false;
    }

    public Vector2 CalculateDirection(TrunkFSM trunk) {
        Vector2 direction;
        if (trunk.transform.eulerAngles.y == 0) {
            direction = Vector2.left;
        }
        else {
            direction = Vector2.right;
        }
        return direction;
    }

    public bool PlayerIsOnSight(TrunkFSM trunk) {
        Vector2 direction = CalculateDirection(trunk);
        foreach (var frontTransform in trunk.frontTransforms) {
            RaycastHit2D[] frontRay = Physics2D.RaycastAll(frontTransform.position, direction, trunk.playerRayDistance);

            foreach (var obj in frontRay) {
                if (obj.collider != null && obj.collider.CompareTag("Ground")) return false;
                if (obj.collider != null && obj.collider.CompareTag("Obstacle")) return false;
                if (obj.collider != null && obj.collider.CompareTag("Gate")) return false;
                if (obj.collider != null && obj.collider.CompareTag("Enemy")) return false;
                if (obj.collider != null && obj.collider.CompareTag("Player")) return true;
            }
        }

        return false;
    }

    #endregion
}