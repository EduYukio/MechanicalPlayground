using UnityEngine;

public class BeeAttackingState : BeeBaseState {
    float attackingTimer;

    public override void EnterState(BeeFSM bee) {
        bee.animator.Play("BeeAttack");
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
        attackingTimer = Helper.GetAnimationDuration("BeeAttack", bee.animator) * 0.8f;
    }

    void AttackAction(BeeFSM bee) {
        Transform spawnTransform = bee.bulletSpawnPosition;
        GameObject bullet = MonoBehaviour.Instantiate(bee.beeBullet, spawnTransform.position, bee.transform.rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = bee.bulletDirection * bee.bulletSpeed;

        bee.attackCooldownTimer = bee.startAttackCooldownTimer;
    }
}