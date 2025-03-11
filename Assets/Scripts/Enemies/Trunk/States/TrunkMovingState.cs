using UnityEngine;

public class TrunkMovingState : TrunkBaseState
{
    private float distanceToCheckGround, distanceToCheckObstacle;

    public override void EnterState(TrunkFSM trunk)
    {
        Setup();
        PlayAnimation(trunk);
    }

    public override void Update(TrunkFSM trunk)
    {
        if (base.CheckTransitionToAttacking(trunk)) return;

        base.MoveAction(trunk);
        if (CheckTransitionToIdle(trunk)) return;
    }

    private void Setup()
    {
        distanceToCheckGround = 0.2f;
        distanceToCheckObstacle = 0.1f;
    }

    private void PlayAnimation(TrunkFSM trunk)
    {
        trunk.animator.Play("Moving");
    }

    private bool ThereIsGroundToWalk(TrunkFSM trunk)
    {
        RaycastHit2D[] groundRay = Physics2D.RaycastAll(trunk.groundTransform.position, Vector2.down, distanceToCheckGround);

        foreach (var obj in groundRay)
        {
            if (obj.collider != null && obj.collider.CompareTag("Ground"))
            {
                return true;
            }
        }
        return false;
    }

    private bool ReachedObstacle(TrunkFSM trunk)
    {
        Vector2 direction = base.CalculateDirection(trunk);

        foreach (var frontTransform in trunk.frontTransforms)
        {
            int layersToCollide = LayerMask.GetMask("Ground", "Obstacles", "Gate", "Enemies");
            RaycastHit2D ray = Physics2D.Raycast(frontTransform.position, direction, distanceToCheckObstacle, layersToCollide);
            if (ray.collider == null) continue;

            string[] obstacleTags = { "Ground", "Obstacle", "Gate", "Enemy" };
            foreach (var tag in obstacleTags)
            {
                if (ray.collider.CompareTag(tag)) return true;
            }
        }

        return false;
    }

    public override bool CheckTransitionToIdle(TrunkFSM trunk)
    {
        if (!ThereIsGroundToWalk(trunk) || ReachedObstacle(trunk))
        {
            trunk.needToTurn = true;
            trunk.TransitionToState(trunk.IdleState);
            return true;
        }

        return false;
    }
}