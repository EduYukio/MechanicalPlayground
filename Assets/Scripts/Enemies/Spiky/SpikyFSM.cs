using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpikyFSM : Enemy {
    private SpikyBaseState currentState;

    public readonly SpikyIdleState IdleState = new SpikyIdleState();
    public readonly SpikyAttackingState AttackingState = new SpikyAttackingState();
    public readonly SpikyBeingHitState BeingHitState = new SpikyBeingHitState();
    public readonly SpikyDyingState DyingState = new SpikyDyingState();

    public GameObject bullet;
    public Transform bulletSpawnPosition;
    public float bulletSpeed = 2f;
    public float moveSpeed = 3f;
    public float distanceToMove = 3f;
    public float startAttackCooldownTimer = 1.5f;
    public float attackCooldownTimer = 0;
    public Vector2 bulletDirection = Vector2.down;
    [HideInInspector] public float initialY;
    [HideInInspector] public Vector2 targetPosition;
    [HideInInspector] public bool isBeingHit = false;
    [HideInInspector] public SpriteRenderer spriteRenderer;

    private void Start() {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        MoveSetup();
        attackCooldownTimer = startAttackCooldownTimer;
        TransitionToState(IdleState);
    }

    private void Update() {
        ProcessTimers();

        currentState.Update(this);
    }

    public void TransitionToState(SpikyBaseState state) {
        currentState = state;
        currentState.EnterState(this);
    }

    public override void TakeDamage(float damage) {
        isBeingHit = true;
        currentHealth -= damage;
    }

    void MoveSetup() {
        initialY = transform.position.y;
        targetPosition = new Vector2(transform.position.x, initialY + distanceToMove);
    }

    private void ProcessTimers() {
        float step = Time.deltaTime;
        if (attackCooldownTimer >= 0) attackCooldownTimer -= step;
    }
}