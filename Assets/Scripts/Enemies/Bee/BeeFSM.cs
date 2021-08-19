using UnityEngine;

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
    [HideInInspector] public float bulletTimerSyncedWithAnimation;
    [HideInInspector] public float initialCoord;
    [HideInInspector] public Vector2 targetPosition;
    [HideInInspector] public SpriteRenderer spriteRenderer;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        bulletTimerSyncedWithAnimation = Helper.GetAnimationDuration("Attacking", animator) * 0.625f;
        currentHealth = maxHealth;
        attackCooldownTimer = 0f;

        PreInstantiateBullets();
        MoveSetup();
        TransitionToState(MovingState);
    }

    private void Update() {
        currentState.Update(this);
    }

    private void FixedUpdate() {
        ProcessTimers();
        currentState.FixedUpdate(this);
    }

    public void TransitionToState(BeeBaseState state) {
        currentState = state;
        currentState.EnterState(this);
    }

    private void ProcessTimers() {
        float step = Time.deltaTime;
        if (attackCooldownTimer >= 0) attackCooldownTimer -= step;
    }

    public override void TakeDamage(float damage) {
        base.TakeDamage(damage);
        TransitionToState(BeingHitState);
    }

    private void MoveSetup() {
        if (moveVertically) {
            initialCoord = transform.position.y;
            targetPosition = new Vector2(transform.position.x, initialCoord + distanceToMove);
        }
        else {
            initialCoord = transform.position.x;
            targetPosition = new Vector2(initialCoord + distanceToMove, transform.position.y);
        }
    }

    private void PreInstantiateBullets() {
        float maxLength = CalculateMaxRayLength();

        float deltaT = startAttackCooldownTimer;
        float distance = bulletSpeed * deltaT;

        Vector3 direction = CalculateDirection();
        float timeStep = bulletTimerSyncedWithAnimation + startAttackCooldownTimer;
        Vector3 initialPosition = bulletSpawnTransform.position;
        while (distance < maxLength) {
            Vector3 spawnPosition = initialPosition + direction * distance;
            SpawnBullet(spawnPosition);
            deltaT += timeStep;
            distance = bulletSpeed * deltaT;
        }
    }

    public void SpawnBullet(Vector3 spawnPosition) {
        GameObject bullet = MonoBehaviour.Instantiate(bulletPrefab, spawnPosition, transform.rotation);
        Vector3 direction = CalculateDirection();
        bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
    }

    private float CalculateMaxRayLength() {
        float arbitraryMaxLength = 100f;

        RaycastHit2D ray = ThrowRayCast(arbitraryMaxLength);
        if (ray.collider == null) return arbitraryMaxLength;

        bool isGround = ray.collider.CompareTag("Ground");
        bool isObstacle = ray.collider.CompareTag("Obstacle");
        bool isGate = ray.collider.CompareTag("Gate");

        bool hitBlockable = isGround || isObstacle || isGate;
        if (hitBlockable) return ray.distance;

        return arbitraryMaxLength;
    }

    private RaycastHit2D ThrowRayCast(float maxLenght) {
        Vector3 direction = CalculateDirection();
        int layersToCollide = LayerMask.GetMask("Ground", "Obstacles", "Gate");
        if (gameObject.name.Contains("Red")) {
            layersToCollide = LayerMask.GetMask("Gate");
        }

        RaycastHit2D ray = Physics2D.Raycast(bulletSpawnTransform.position, direction, maxLenght, layersToCollide);

        return ray;
    }

    private Vector3 CalculateDirection() {
        return (bulletDirectionTransform.position - bulletSpawnTransform.position).normalized;
    }
}