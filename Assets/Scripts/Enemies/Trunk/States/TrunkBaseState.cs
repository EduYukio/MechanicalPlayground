﻿using UnityEngine;

public abstract class TrunkBaseState
{
    public abstract void EnterState(TrunkFSM trunk);
    public virtual void Update(TrunkFSM trunk) { }
    public virtual void FixedUpdate(TrunkFSM trunk) { }

    public virtual bool CheckTransitionToIdle(TrunkFSM trunk)
    {
        trunk.TransitionToState(trunk.IdleState);
        return true;
    }

    public virtual bool CheckTransitionToMoving(TrunkFSM trunk)
    {
        InvertDirectionIfNeeded(trunk);

        trunk.TransitionToState(trunk.MovingState);
        return true;
    }

    public virtual bool CheckTransitionToAttacking(TrunkFSM trunk)
    {
        if (trunk.attackCooldownTimer <= 0 && PlayerIsOnSight(trunk))
        {
            trunk.TransitionToState(trunk.AttackingState);
            return true;
        }
        return false;
    }

    public virtual bool CheckTransitionToDying(TrunkFSM trunk)
    {
        if (trunk.currentHealth <= 0)
        {
            trunk.TransitionToState(trunk.DyingState);
            return true;
        }

        return false;
    }



    public void MoveAction(TrunkFSM trunk)
    {
        trunk.transform.Translate(Vector2.left * trunk.moveSpeed * Time.deltaTime);
    }

    public void InvertDirectionIfNeeded(TrunkFSM trunk)
    {
        if (trunk.needToTurn)
        {
            if (trunk.transform.eulerAngles.y == 0)
            {
                trunk.transform.eulerAngles = new Vector3(0f, 180f, 0f);
            }
            else if (trunk.transform.eulerAngles.y == 180)
            {
                trunk.transform.eulerAngles = new Vector3(0f, 0f, 0f);
            }
        }

        trunk.needToTurn = false;
    }

    public bool PlayerIsOnSight(TrunkFSM trunk)
    {
        Vector2 direction = CalculateDirection(trunk);

        foreach (var frontTransform in trunk.frontTransforms)
        {
            int layersToCollide = LayerMask.GetMask("Player", "Ground", "Obstacles", "Gate", "Enemies");
            RaycastHit2D ray = Physics2D.Raycast(frontTransform.position, direction, trunk.playerRayDistance, layersToCollide);

            if (ray.collider == null) continue;
            if (ray.collider.CompareTag("Player")) return true;
        }

        return false;
    }

    public Vector2 CalculateDirection(TrunkFSM trunk)
    {
        return (trunk.bulletDirectionTransform.position - trunk.bulletSpawnTransform.position).normalized;
    }
}