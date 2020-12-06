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
        Transform[] end = spiky.bulletEndTransforms;
        Transform[] start = spiky.bulletStartTransforms;
        for (int i = 0; i < bulletDirections.Length; i++) {
            bulletDirections[i] = (end[i].position - start[i].position).normalized;
        }
    }

    void AttackAction(SpikyFSM spiky) {
        for (int i = 0; i < bulletDirections.Length; i++) {
            Transform spawnTransform = spiky.bulletStartTransforms[i];
            GameObject bullet = MonoBehaviour.Instantiate(spiky.bullet, spawnTransform.position, Quaternion.identity);
            bullet.transform.eulerAngles = spawnTransform.eulerAngles;
            bullet.GetComponent<Rigidbody2D>().velocity = bulletDirections[i] * spiky.bulletSpeed;
        }

        spiky.attackCooldownTimer = spiky.startAttackCooldownTimer;
    }
}