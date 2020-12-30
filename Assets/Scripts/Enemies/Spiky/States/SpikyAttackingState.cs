using UnityEngine;

public class SpikyAttackingState : SpikyBaseState {
    float attackingTimer;
    Vector2[] bulletDirections = new Vector2[5];

    public override void EnterState(SpikyFSM spiky) {
        spiky.animator.Play("Attacking");
        Setup(spiky);
    }

    public override void Update(SpikyFSM spiky) {
        if (base.CheckTransitionToBeingHit(spiky)) return;

        if (attackingTimer >= 0) {
            attackingTimer -= Time.deltaTime;
            return;
        }

        AttackAction(spiky);

        if (base.CheckTransitionToIdle(spiky)) return;
    }

    void Setup(SpikyFSM spiky) {
        attackingTimer = spiky.bulletSpawnTimerSyncedWithAnimation;
    }

    void AttackAction(SpikyFSM spiky) {
        Vector3[] bulletDirections = spiky.CalculateDirections();

        for (int i = 0; i < bulletDirections.Length; i++) {
            Vector3 newPos = spiky.bulletStartTransforms[i].position;
            Vector3 newAngle = spiky.bulletStartTransforms[i].eulerAngles;
            spiky.SpawnBullet(newPos, newAngle, bulletDirections[i]);
        }
        spiky.attackCooldownTimer = spiky.startAttackCooldownTimer;
    }
}