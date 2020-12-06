using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeeFSM : Enemy {
    private BeeBaseState currentState;

    public readonly BeeMovingState MovingState = new BeeMovingState();
    public readonly BeeAttackingState AttackingState = new BeeAttackingState();
    public readonly BeeBeingHitState BeingHitState = new BeeBeingHitState();
    public readonly BeeDyingState DyingState = new BeeDyingState();

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
        TransitionToState(MovingState);
    }

    private void Update() {
        ProcessTimers();

        currentState.Update(this);
    }

    public void TransitionToState(BeeBaseState state) {
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