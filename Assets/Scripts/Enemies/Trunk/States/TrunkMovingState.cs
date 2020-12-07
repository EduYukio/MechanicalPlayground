using UnityEngine;

public class TrunkMovingState : TrunkBaseState {
    float distanceToCheckGround = 0.2f;
    float distanceToCheckObstacle = 0.2f;

    public override void EnterState(TrunkFSM trunk) {
        trunk.animator.Play("Moving");
        Debug.Log("Entered moving");
    }

    public override void Update(TrunkFSM trunk) {
        // if (base.CheckTransitionToBeingHit(trunk)) return;
        // if (base.CheckTransitionToAttacking(trunk)) return;

        base.MoveAction(trunk);
        if (CheckTransitionToIdle(trunk)) return;
    }

    bool CheckIfReachedEndOfPlatform(TrunkFSM trunk) {
        RaycastHit2D groundRay = Physics2D.Raycast(trunk.groundTransform.position, Vector2.down, distanceToCheckGround);
        bool? isOnGround = groundRay.collider?.CompareTag("Ground");
        if (isOnGround == false || isOnGround == null) return true;
        else return false;
    }

    bool CheckIfReachedObstacle(TrunkFSM trunk) {
        Vector2 direction;
        if (trunk.transform.eulerAngles.y == 0) {
            direction = Vector2.left;
        }
        else {
            direction = Vector2.right;
        }
        RaycastHit2D frontRay = Physics2D.Raycast(trunk.frontTransform.position, direction, distanceToCheckObstacle);
        if (frontRay.collider == null) {
            return false;
        }
        else if (frontRay.collider.CompareTag("Player")) {
            return false;
        }
        else if (frontRay.collider.CompareTag("Checker")) {
            return false;
        }

        return true;
    }

    public override bool CheckTransitionToIdle(TrunkFSM trunk) {
        if (CheckIfReachedEndOfPlatform(trunk) || CheckIfReachedObstacle(trunk)) {
            trunk.needToTurn = true;
            trunk.TransitionToState(trunk.IdleState);
            return true;
        };

        return false;
    }
}