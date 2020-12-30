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
    [HideInInspector] public bool isBeingHit = false;
    [HideInInspector] public SpriteRenderer spriteRenderer;

    private void Start() {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

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
        isBeingHit = true;
        currentHealth -= damage;
    }

    private void ProcessTimers() {
        float step = Time.deltaTime;
        if (attackCooldownTimer >= 0) attackCooldownTimer -= step;
    }

    public void SpawnBullets(Vector3[] spawnPositions, Vector3[] spawnAngles) {
        Vector2[] bulletDirections = CalculateDirections();

        for (int i = 0; i < bulletDirections.Length; i++) {
            GameObject bullet = MonoBehaviour.Instantiate(bulletPrefab, spawnPositions[i], Quaternion.identity);
            bullet.transform.eulerAngles = spawnAngles[i];
            bullet.GetComponent<Rigidbody2D>().velocity = bulletDirections[i] * bulletSpeed;
        }
    }

    Vector2[] CalculateDirections() {
        Vector2[] bulletDirections = new Vector2[5];
        Transform[] end = bulletEndTransforms;
        Transform[] start = bulletStartTransforms;

        for (int i = 0; i < bulletDirections.Length; i++) {
            bulletDirections[i] = (end[i].position - start[i].position).normalized;
        }

        return bulletDirections;
    }
}