using UnityEngine;

public class BeeBeingHitState : BeeBaseState
{
    private float hitTimer;

    public override void EnterState(BeeFSM bee)
    {
        Setup(bee);
        PlayAnimation(bee);
        PlayAudio();
        BeingHitAction(bee);
    }

    public override void FixedUpdate(BeeFSM bee)
    {
        if (base.CheckTransitionToDying(bee)) return;

        if (hitTimer >= 0)
        {
            hitTimer -= Time.deltaTime;
            return;
        }

        if (base.CheckTransitionToMoving(bee)) return;
    }

    private void Setup(BeeFSM bee)
    {
        hitTimer = Helper.GetAnimationDuration("BeingHit", bee.animator);
    }

    private void PlayAnimation(BeeFSM bee)
    {
        bee.animator.Play("BeingHit", -1, 0f);
    }

    private void PlayAudio()
    {
        Manager.audio.Play("EnemyHit");
    }

    private void BeingHitAction(BeeFSM bee)
    {
        bee.rb.velocity = Vector2.zero;
    }
}