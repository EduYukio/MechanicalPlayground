using UnityEngine;

public class TrunkBeingHitState : TrunkBaseState {
    private float hitTimer;

    public override void EnterState(TrunkFSM trunk) {
        Setup(trunk);
        PlayAnimation(trunk);
        PlayAudio();
        BeingHitAction(trunk);
    }

    public override void FixedUpdate(TrunkFSM trunk) {
        if (base.CheckTransitionToDying(trunk)) return;

        if (hitTimer >= 0) {
            hitTimer -= Time.deltaTime;
            return;
        }

        if (base.CheckTransitionToAttacking(trunk)) return;
        if (CheckTransitionToMoving(trunk)) return;
    }

    private void Setup(TrunkFSM trunk) {
        hitTimer = Helper.GetAnimationDuration("BeingHit", trunk.animator);
    }

    private void PlayAnimation(TrunkFSM trunk) {
        trunk.animator.Play("BeingHit", -1, 0f);
    }

    private void PlayAudio() {
        Manager.audio.Play("EnemyHit");
    }

    private void BeingHitAction(TrunkFSM trunk) {
        trunk.rb.velocity = Vector2.zero;
    }

    public override bool CheckTransitionToMoving(TrunkFSM trunk) {
        if (trunk.attackCooldownTimer > 0) return false;
        return (base.CheckTransitionToMoving(trunk));
    }
}