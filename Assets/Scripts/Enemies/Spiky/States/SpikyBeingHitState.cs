using UnityEngine;

public class SpikyBeingHitState : SpikyBaseState
{
    private float hitTimer;

    public override void EnterState(SpikyFSM spiky)
    {
        Setup(spiky);
        PlayAnimation(spiky);
        PlayAudio();
        BeingHitAction(spiky);
    }

    public override void FixedUpdate(SpikyFSM spiky)
    {
        if (base.CheckTransitionToDying(spiky)) return;

        if (hitTimer >= 0)
        {
            hitTimer -= Time.deltaTime;
            return;
        }

        if (base.CheckTransitionToIdle(spiky)) return;
    }

    private void Setup(SpikyFSM spiky)
    {
        hitTimer = Helper.GetAnimationDuration("BeingHit", spiky.animator);
    }

    private void PlayAnimation(SpikyFSM spiky)
    {
        spiky.animator.Play("BeingHit", -1, 0f);
    }

    private void PlayAudio()
    {
        Manager.audio.Play("EnemyHit");
    }

    private void BeingHitAction(SpikyFSM spiky)
    {
        spiky.rb.velocity = Vector2.zero;
    }
}