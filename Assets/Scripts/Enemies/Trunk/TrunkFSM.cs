using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class TrunkFSM : Enemy {
    private TrunkBaseState currentState;

    public readonly TrunkIdleState IdleState = new TrunkIdleState();
    public readonly TrunkAttackingState AttackingState = new TrunkAttackingState();
    public readonly TrunkBeingHitState BeingHitState = new TrunkBeingHitState();
    public readonly TrunkDyingState DyingState = new TrunkDyingState();
    public readonly TrunkMovingState MovingState = new TrunkMovingState();

    public float moveSpeed = 3f;
    public GameObject bulletPrefab;
    public Transform bulletSpawnTransform;
    public Transform bulletDirectionTransform;
    public float bulletSpeed = 2f;
    public float startAttackCooldownTimer = 1.5f;
    public float attackCooldownTimer = 0;
    public float playerRayDistance = 5f;
    public Transform groundTransform;
    public Transform[] frontTransforms;
    public bool needToTurn = false;
    [HideInInspector] public float bulletSpawnTimerSyncedWithAnimation;
    [HideInInspector] public SpriteRenderer spriteRenderer;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        bulletSpawnTimerSyncedWithAnimation = Helper.GetAnimationDuration("Attacking", animator) * 0.7f;
    }

    private void Start() {
        currentHealth = maxHealth;
        attackCooldownTimer = 0f;
        TransitionToState(IdleState);
    }

    private void Update() {
        ProcessTimers();

        currentState.Update(this);
    }

    public void TransitionToState(TrunkBaseState state) {
        currentState = state;
        currentState.EnterState(this);
    }

    public override void TakeDamage(float damage) {
        currentHealth -= damage;
        TransitionToState(BeingHitState);
    }

    private void ProcessTimers() {
        float step = Time.deltaTime;
        if (attackCooldownTimer >= 0) attackCooldownTimer -= step;
    }

    public void SpawnBullet(Vector3 spawnPosition) {
        GameObject bullet = MonoBehaviour.Instantiate(bulletPrefab, spawnPosition, transform.rotation);
        Vector2 direction = (bulletDirectionTransform.position - bulletSpawnTransform.position).normalized;
        bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
    }
}