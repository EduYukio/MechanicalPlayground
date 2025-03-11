using UnityEngine;

public class SpikyAttackingState : SpikyBaseState
{
    private float attackingTimer;
    private Vector2[] bulletDirections;

    public override void EnterState(SpikyFSM spiky)
    {
        Setup(spiky);
        PlayAnimation(spiky);
    }

    public override void FixedUpdate(SpikyFSM spiky)
    {
        if (attackingTimer >= 0)
        {
            attackingTimer -= Time.deltaTime;
            return;
        }

        AttackAction(spiky);
        if (base.CheckTransitionToIdle(spiky)) return;
    }

    private void Setup(SpikyFSM spiky)
    {
        attackingTimer = spiky.bulletSpawnTimerSyncedWithAnimation;
        bulletDirections = new Vector2[5];
    }

    private void PlayAnimation(SpikyFSM spiky)
    {
        spiky.animator.Play("Attacking");
    }

    private void AttackAction(SpikyFSM spiky)
    {
        if (spiky.spriteRenderer.isVisible) Manager.audio.Play("Enemy Shoot");
        Vector3[] bulletDirections = spiky.CalculateDirections();

        for (int i = 0; i < bulletDirections.Length; i++)
        {
            Vector3 position = spiky.bulletStartTransforms[i].position;
            Vector3 angle = spiky.bulletStartTransforms[i].eulerAngles;
            spiky.SpawnBullet(position, angle, bulletDirections[i]);
        }

        spiky.attackCooldownTimer = spiky.startAttackCooldownTimer;
    }
}