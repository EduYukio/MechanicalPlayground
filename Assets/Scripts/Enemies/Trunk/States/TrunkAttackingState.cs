using UnityEngine;

public class TrunkAttackingState : TrunkBaseState {
    float attackingTimer;

    public override void EnterState(TrunkFSM trunk) {
        trunk.animator.Play("Attacking");
        Setup(trunk);
    }

    public override void Update(TrunkFSM trunk) {
        if (attackingTimer >= 0) {
            attackingTimer -= Time.deltaTime;
            return;
        }

        AttackAction(trunk);

        if (base.CheckTransitionToIdle(trunk)) return;
    }

    void Setup(TrunkFSM trunk) {
        attackingTimer = trunk.bulletSpawnTimerSyncedWithAnimation;
        trunk.rb.velocity = Vector2.zero;
    }

    void AttackAction(TrunkFSM trunk) {
        if (trunk.spriteRenderer.isVisible) Manager.audio.Play("Enemy Shoot");
        trunk.SpawnBullet(trunk.bulletSpawnTransform.position);
        trunk.attackCooldownTimer = trunk.startAttackCooldownTimer;
    }
}