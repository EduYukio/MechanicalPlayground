using UnityEngine;

public class TrunkAttackingState : TrunkBaseState {
    private float attackingTimer;

    public override void EnterState(TrunkFSM trunk) {
        Setup(trunk);
        PlayAnimation(trunk);
    }

    public override void FixedUpdate(TrunkFSM trunk) {
        if (attackingTimer >= 0) {
            attackingTimer -= Time.deltaTime;
            return;
        }

        AttackAction(trunk);
        if (base.CheckTransitionToIdle(trunk)) return;
    }

    private void Setup(TrunkFSM trunk) {
        attackingTimer = trunk.bulletSpawnTimerSyncedWithAnimation;
        trunk.rb.velocity = Vector2.zero;
    }

    private void PlayAnimation(TrunkFSM trunk) {
        trunk.animator.Play("Attacking");
    }

    private void AttackAction(TrunkFSM trunk) {
        if (trunk.spriteRenderer.isVisible) Manager.audio.Play("Enemy Shoot");
        trunk.SpawnBullet(trunk.bulletSpawnTransform.position);
        trunk.attackCooldownTimer = trunk.startAttackCooldownTimer;
    }
}