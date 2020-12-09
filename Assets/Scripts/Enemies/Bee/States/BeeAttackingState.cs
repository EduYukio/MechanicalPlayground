using UnityEngine;

public class BeeAttackingState : BeeBaseState {
    float attackingTimer;

    public override void EnterState(BeeFSM bee) {
        bee.animator.Play("Attacking");
        Setup(bee);
    }

    public override void Update(BeeFSM bee) {
        if (base.CheckTransitionToBeingHit(bee)) return;

        base.MoveAction(bee);

        if (attackingTimer >= 0) {
            attackingTimer -= Time.deltaTime;
            return;
        }

        AttackAction(bee);

        if (base.CheckTransitionToMoving(bee)) return;
    }

    void Setup(BeeFSM bee) {
        attackingTimer = Helper.GetAnimationDuration("Attacking", bee.animator) * 0.8f;
    }

    void AttackAction(BeeFSM bee) {
        Transform spawnTransform = bee.bulletSpawnPosition;
        GameObject bullet = MonoBehaviour.Instantiate(bee.bullet, spawnTransform.position, bee.transform.rotation);
        Vector2 direction = (bee.bulletDirection.position - bee.bulletSpawnPosition.position).normalized;

        bullet.GetComponent<Rigidbody2D>().velocity = direction * bee.bulletSpeed;

        bee.attackCooldownTimer = bee.startAttackCooldownTimer;
    }
}