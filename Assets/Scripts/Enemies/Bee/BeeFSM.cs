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

    public GameObject bulletPrefab;
    public Transform bulletSpawnTransform;
    public Transform bulletDirectionTransform;
    public float bulletSpeed = 2f;
    public float moveSpeed = 3f;
    public float distanceToMove = 3f;
    public bool moveVertically = true;
    public float startAttackCooldownTimer = 1.5f;
    public float attackCooldownTimer = 0;
    [HideInInspector] public float bulletSpawnTimerSyncedWithAnimation;
    [HideInInspector] public float initialCoord;
    [HideInInspector] public Vector2 targetPosition;
    [HideInInspector] public bool isBeingHit = false;
    [HideInInspector] public SpriteRenderer spriteRenderer;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        bulletSpawnTimerSyncedWithAnimation = Helper.GetAnimationDuration("Attacking", animator) * 0.625f;
        PreInstantiateBullets();
    }

    private void Start() {
        currentHealth = maxHealth;

        MoveSetup();
        attackCooldownTimer = 0f;
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
        if (moveVertically) {
            initialCoord = transform.position.y;
            targetPosition = new Vector2(transform.position.x, initialCoord + distanceToMove);
        }
        else {
            initialCoord = transform.position.x;
            targetPosition = new Vector2(initialCoord + distanceToMove, transform.position.y);
        }
    }

    private void ProcessTimers() {
        float step = Time.deltaTime;
        if (attackCooldownTimer >= 0) attackCooldownTimer -= step;
    }

    public void SpawnBullet(Vector3 spawnPosition) {
        GameObject bullet = MonoBehaviour.Instantiate(bulletPrefab, spawnPosition, transform.rotation);
        Vector3 direction = CalculateDirection();
        bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
    }

    void PreInstantiateBullets() {
        float maxLength = CalculateMaxRayLength();

        float delta_t = startAttackCooldownTimer;
        float distance = bulletSpeed * delta_t;

        Vector3 direction = CalculateDirection();
        float timeStep = bulletSpawnTimerSyncedWithAnimation + startAttackCooldownTimer;
        Vector3 initialPosition = bulletSpawnTransform.position;
        while (distance < maxLength) {
            Vector3 spawnPosition = initialPosition + direction * distance;
            SpawnBullet(spawnPosition);
            delta_t += timeStep;
            distance = bulletSpeed * delta_t;
        }
    }

    float CalculateMaxRayLength() {
        float arbitraryMaxLength = 100f;
        int layersToCollide = LayerMask.GetMask("Ground", "Obstacles", "Gate");

        Vector3 direction = CalculateDirection();
        RaycastHit2D frontRay = Physics2D.Raycast(bulletSpawnTransform.position, direction, arbitraryMaxLength, layersToCollide);

        if (frontRay.collider == null) {
            return arbitraryMaxLength;
        }

        bool isGround = frontRay.collider.CompareTag("Ground");
        bool isObstacle = frontRay.collider.CompareTag("Obstacle");
        bool isGate = frontRay.collider.CompareTag("Gate");

        bool hitBlockable = isGround || isObstacle || isGate;

        if (hitBlockable) {
            return frontRay.distance;
        }

        return arbitraryMaxLength;
    }

    Vector3 CalculateDirection() {
        return (bulletDirectionTransform.position - bulletSpawnTransform.position).normalized;
    }
}