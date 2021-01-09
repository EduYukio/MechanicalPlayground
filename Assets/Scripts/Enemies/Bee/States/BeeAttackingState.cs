using UnityEngine;

public class BeeAttackingState : BeeBaseState {
    float attackingTimer;

    public override void EnterState(BeeFSM bee) {
        bee.animator.Play("Attacking");
        Setup(bee);
    }

    public override void Update(BeeFSM bee) {
        base.MoveAction(bee);

        if (attackingTimer >= 0) {
            attackingTimer -= Time.deltaTime;
            return;
        }

        AttackAction(bee);

        if (base.CheckTransitionToMoving(bee)) return;
    }

    void Setup(BeeFSM bee) {
        attackingTimer = bee.bulletSpawnTimerSyncedWithAnimation;
    }

    void AttackAction(BeeFSM bee) {
        if (bee.spriteRenderer.isVisible) Manager.audio.Play("Enemy Shoot");
        bee.SpawnBullet(bee.bulletSpawnTransform.position);
        bee.attackCooldownTimer = bee.startAttackCooldownTimer;
    }
}