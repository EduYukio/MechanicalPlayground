using UnityEngine;

public class TrunkMovingState : TrunkBaseState {
    private float distanceToCheckGround = 0.2f;
    private float distanceToCheckObstacle = 0.1f;

    public override void EnterState(TrunkFSM trunk) {
        trunk.animator.Play("Moving");
    }

    public override void Update(TrunkFSM trunk) {
        if (base.CheckTransitionToAttacking(trunk)) return;

        base.MoveAction(trunk);
        if (CheckTransitionToIdle(trunk)) return;
    }

    private bool ThereIsGroundToWalk(TrunkFSM trunk) {
        RaycastHit2D[] groundRay = Physics2D.RaycastAll(trunk.groundTransform.position, Vector2.down, distanceToCheckGround);

        foreach (var obj in groundRay) {
            if (obj.collider != null && obj.collider.CompareTag("Ground")) {
                return true;
            }
        }
        return false;
    }

    private bool ReachedObstacle(TrunkFSM trunk) {
        Vector2 direction = CalculateDirection(trunk);
        foreach (var frontTransform in trunk.frontTransforms) {
            RaycastHit2D[] frontRay = Physics2D.RaycastAll(frontTransform.position, direction, distanceToCheckObstacle);

            foreach (var obj in frontRay) {
                if (obj.collider != null) {
                    bool isWall = obj.collider.CompareTag("Ground");
                    bool isEnemy = obj.collider.CompareTag("Enemy");
                    bool isObstacle = obj.collider.CompareTag("Obstacle");
                    bool isGate = obj.collider.CompareTag("Gate");

                    bool hasHitObstacle = isObstacle || isWall || isEnemy || isGate;
                    if (hasHitObstacle) {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public override bool CheckTransitionToIdle(TrunkFSM trunk) {
        if (!ThereIsGroundToWalk(trunk) || ReachedObstacle(trunk)) {
            trunk.needToTurn = true;
            trunk.TransitionToState(trunk.IdleState);
            return true;
        }

        return false;
    }
}