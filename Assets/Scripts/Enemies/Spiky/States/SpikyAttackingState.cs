using UnityEngine;

public class SpikyAttackingState : SpikyBaseState {
    private float attackingTimer;
    private Vector2[] bulletDirections = new Vector2[5];

    public override void EnterState(SpikyFSM spiky) {
        spiky.animator.Play("Attacking");
        Setup(spiky);
    }

    public override void Update(SpikyFSM spiky) {
        if (attackingTimer >= 0) {
            attackingTimer -= Time.deltaTime;
            return;
        }

        AttackAction(spiky);

        if (base.CheckTransitionToIdle(spiky)) return;
    }

    private void Setup(SpikyFSM spiky) {
        attackingTimer = spiky.bulletSpawnTimerSyncedWithAnimation;
    }

    private void AttackAction(SpikyFSM spiky) {
        if (spiky.spriteRenderer.isVisible) Manager.audio.Play("Enemy Shoot");
        Vector3[] bulletDirections = spiky.CalculateDirections();

        for (int i = 0; i < bulletDirections.Length; i++) {
            Vector3 newPos = spiky.bulletStartTransforms[i].position;
            Vector3 newAngle = spiky.bulletStartTransforms[i].eulerAngles;
            spiky.SpawnBullet(newPos, newAngle, bulletDirections[i]);
        }
        spiky.attackCooldownTimer = spiky.startAttackCooldownTimer;
    }
}