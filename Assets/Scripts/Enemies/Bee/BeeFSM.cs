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

    [HideInInspector] public SpriteRenderer spriteRenderer;

    public float moveSpeed = 3f;
    public float distanceToMove = 3f;
    public float initialY;
    public Vector2 targetPosition;
    public bool isBeingHit = false;

    private void Start() {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        MoveSetup();
        TransitionToState(MovingState);
    }

    private void Update() {
        currentState.Update(this);
    }

    public void TransitionToState(BeeBaseState state) {
        currentState = state;
        currentState.EnterState(this);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        KillPlayer(other);
    }

    void KillPlayer(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            PlayerFSM player = other.gameObject.GetComponent<PlayerFSM>();
            player.TransitionToState(player.DyingState);
        }
    }

    public override void TakeDamage(float damage) {
        isBeingHit = true;
        currentHealth -= damage;
    }

    void MoveSetup() {
        initialY = transform.position.y;
        targetPosition = new Vector2(transform.position.x, initialY + distanceToMove);
    }
}