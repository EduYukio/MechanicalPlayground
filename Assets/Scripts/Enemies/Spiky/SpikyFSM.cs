using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class SpikyFSM : Enemy {
    private SpikyBaseState currentState;

    public readonly SpikyIdleState IdleState = new SpikyIdleState();
    public readonly SpikyAttackingState AttackingState = new SpikyAttackingState();
    public readonly SpikyBeingHitState BeingHitState = new SpikyBeingHitState();
    public readonly SpikyDyingState DyingState = new SpikyDyingState();

    public GameObject bulletPrefab;
    public Transform[] bulletStartTransforms;
    public Transform[] bulletEndTransforms;
    public float bulletSpeed = 2f;
    public float startAttackCooldownTimer = 1.5f;
    public float attackCooldownTimer = 0;
    [HideInInspector] public float bulletSpawnTimerSyncedWithAnimation;
    [HideInInspector] public SpriteRenderer spriteRenderer;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        bulletSpawnTimerSyncedWithAnimation = Helper.GetAnimationDuration("Attacking", animator);
        PreInstantiateBullets();
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

    public void TransitionToState(SpikyBaseState state) {
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

    public void SpawnBullet(Vector3 spawnPosition, Vector3 spawnAngle, Vector3 direction) {
        GameObject bullet = MonoBehaviour.Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
        bullet.transform.eulerAngles = spawnAngle;
        bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
    }

    public Vector3[] CalculateDirections() {
        Vector3[] bulletDirections = new Vector3[5];
        Transform[] end = bulletEndTransforms;
        Transform[] start = bulletStartTransforms;

        for (int i = 0; i < bulletDirections.Length; i++) {
            bulletDirections[i] = (end[i].position - start[i].position).normalized;
        }

        return bulletDirections;
    }

    void PreInstantiateBullets() {
        Vector3[] directions = CalculateDirections();

        for (int i = 0; i < directions.Length; i++) {
            Vector3 initialPosition = bulletStartTransforms[i].position;
            float maxLength = CalculateMaxRayLength(initialPosition, directions[i]);

            float delta_t = startAttackCooldownTimer;
            float distance = bulletSpeed * delta_t;

            float timeStep = bulletSpawnTimerSyncedWithAnimation + startAttackCooldownTimer;
            while (distance < maxLength) {
                Vector3 spawnPosition = initialPosition + directions[i] * distance;
                Vector3 spawnAngle = bulletStartTransforms[i].eulerAngles;
                SpawnBullet(spawnPosition, spawnAngle, directions[i]);
                delta_t += timeStep;
                distance = bulletSpeed * delta_t;
            }
        }
    }

    float CalculateMaxRayLength(Vector3 initialPosition, Vector3 direction) {
        float arbitraryMaxLength = 100f;
        int layersToCollide = LayerMask.GetMask("Ground", "Obstacles", "Gate");

        RaycastHit2D frontRay = Physics2D.Raycast(initialPosition, direction, arbitraryMaxLength, layersToCollide);

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
}