using UnityEngine;

public class TrunkAttackingState : TrunkBaseState {
    float attackingTimer;

    public override void EnterState(TrunkFSM trunk) {
        trunk.animator.Play("Attacking");
        Setup(trunk);
    }

    public override void Update(TrunkFSM trunk) {
        if (base.CheckTransitionToBeingHit(trunk)) return;

        if (attackingTimer >= 0) {
            attackingTimer -= Time.deltaTime;
            return;
        }

        AttackAction(trunk);

        if (base.CheckTransitionToIdle(trunk)) return;
    }

    void Setup(TrunkFSM trunk) {
        attackingTimer = Helper.GetAnimationDuration("Attacking", trunk.animator) * 0.7f;
        trunk.rb.velocity = Vector2.zero;
    }

    void AttackAction(TrunkFSM trunk) {
        Vector2 direction = CalculateDirection(trunk);

        Transform spawnTransform = trunk.bulletSpawnPosition;
        GameObject bullet = MonoBehaviour.Instantiate(trunk.bullet, spawnTransform.position, trunk.transform.rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = direction * trunk.bulletSpeed;

        trunk.attackCooldownTimer = trunk.startAttackCooldownTimer;
    }
}