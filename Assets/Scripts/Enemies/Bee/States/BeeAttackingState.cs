using UnityEngine;

public class BeeAttackingState : BeeBaseState {
    private float attackingTimer;

    public override void EnterState(BeeFSM bee) {
        Setup(bee);
        PlayAnimation(bee);
    }

    public override void FixedUpdate(BeeFSM bee) {
        base.MoveAction(bee);

        if (attackingTimer >= 0) {
            attackingTimer -= Time.deltaTime;
            return;
        }

        AttackAction(bee);
        if (base.CheckTransitionToMoving(bee)) return;
    }

    private void Setup(BeeFSM bee) {
        attackingTimer = bee.bulletTimerSyncedWithAnimation;
    }

    private void PlayAnimation(BeeFSM bee) {
        bee.animator.Play("Attacking");
    }

    private void AttackAction(BeeFSM bee) {
        if (bee.spriteRenderer.isVisible) Manager.audio.Play("Enemy Shoot");

        bee.SpawnBullet(bee.bulletSpawnTransform.position);
        bee.attackCooldownTimer = bee.startAttackCooldownTimer;
    }
}