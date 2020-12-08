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
    public GameObject bullet;
    public float bulletSpeed = 2f;
    public float startAttackCooldownTimer = 1.5f;
    public float attackCooldownTimer = 0;
    public Transform groundTransform;
    public Transform frontTransform;
    public bool needToTurn = false;
    [HideInInspector] public bool isBeingHit = false;
    [HideInInspector] public SpriteRenderer spriteRenderer;

    private void Start() {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        attackCooldownTimer = startAttackCooldownTimer;
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
        isBeingHit = true;
        currentHealth -= damage;
    }

    private void ProcessTimers() {
        float step = Time.deltaTime;
        if (attackCooldownTimer >= 0) attackCooldownTimer -= step;
    }
}