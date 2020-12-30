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
        attackingTimer = Helper.GetAnimationDuration("Attacking", spiky.animator);
    }

    void AttackAction(SpikyFSM spiky) {
        Transform[] spawnTransforms = spiky.bulletStartTransforms;
        Vector3[] spawnPositions = new Vector3[5];
        Vector3[] spawnAngles = new Vector3[5];
        for (int i = 0; i < spawnTransforms.Length; i++) {
            spawnPositions[i] = spawnTransforms[i].position;
            spawnAngles[i] = spawnTransforms[i].eulerAngles;
        }
        spiky.SpawnBullets(spawnPositions, spawnAngles);
        spiky.attackCooldownTimer = spiky.startAttackCooldownTimer;
    }
}