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

        // if (base.CheckTransitionToIdle(trunk)) return;
    }

    void Setup(TrunkFSM trunk) {
        attackingTimer = Helper.GetAnimationDuration("Attacking", trunk.animator);
        // Transform[] end = trunk.bulletEndTransforms;
        // Transform[] start = trunk.bulletStartTransforms;
        // for (int i = 0; i < bulletDirections.Length; i++) {
        //     bulletDirections[i] = (end[i].position - start[i].position).normalized;
        // }
    }

    void AttackAction(TrunkFSM trunk) {
        // for (int i = 0; i < bulletDirections.Length; i++) {
        //     Transform spawnTransform = trunk.bulletStartTransforms[i];
        //     GameObject bullet = MonoBehaviour.Instantiate(trunk.bullet, spawnTransform.position, Quaternion.identity);
        //     bullet.transform.eulerAngles = spawnTransform.eulerAngles;
        //     bullet.GetComponent<Rigidbody2D>().velocity = bulletDirections[i] * trunk.bulletSpeed;
        // }

        trunk.attackCooldownTimer = trunk.startAttackCooldownTimer;
    }
}