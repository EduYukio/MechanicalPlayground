using UnityEngine;

public class SpikyAttackingState : SpikyBaseState {
    float attackingTimer;

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

        // AttackAction(spiky);

        if (base.CheckTransitionToIdle(spiky)) return;
    }

    void Setup(SpikyFSM spiky) {
        attackingTimer = Helper.GetAnimationDuration("Attacking", spiky.animator) * 0.8f;
    }

    // void AttackAction(SpikyFSM spiky) {
    //     Transform spawnTransform = spiky.bulletSpawnPosition;
    //     GameObject bullet = MonoBehaviour.Instantiate(spiky.bullet, spawnTransform.position, spiky.transform.rotation);
    //     bullet.GetComponent<Rigidbody2D>().velocity = spiky.bulletDirection * spiky.bulletSpeed;

    //     spiky.attackCooldownTimer = spiky.startAttackCooldownTimer;
    // }
}