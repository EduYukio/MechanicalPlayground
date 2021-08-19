using System.Collections.Generic;
using UnityEngine;

public class PlayerFSM : MonoBehaviour {
    [SerializeField] private PlayerBaseState currentState;

    public readonly PlayerGroundedState GroundedState = new PlayerGroundedState();
    public readonly PlayerJumpingState JumpingState = new PlayerJumpingState();
    public readonly PlayerFallingState FallingState = new PlayerFallingState();
    public readonly PlayerWalkingState WalkingState = new PlayerWalkingState();
    public readonly PlayerDoubleJumpingState DoubleJumpingState = new PlayerDoubleJumpingState();
    public readonly PlayerDashingState DashingState = new PlayerDashingState();
    public readonly PlayerWallSlidingState WallSlidingState = new PlayerWallSlidingState();
    public readonly PlayerWallJumpingState WallJumpingState = new PlayerWallJumpingState();
    public readonly PlayerAttackingState AttackingState = new PlayerAttackingState();
    public readonly PlayerBlinkingState BlinkingState = new PlayerBlinkingState();
    public readonly PlayerDyingState DyingState = new PlayerDyingState();
    public readonly PlayerPogoingState PogoingState = new PlayerPogoingState();
    public readonly PlayerExplodingState ExplodingState = new PlayerExplodingState();
    public readonly PlayerGunBootsState GunBootsState = new PlayerGunBootsState();

    [Header("Debug")]
    [SerializeField] private bool canActivateSlowMotion = false;
    [SerializeField] private bool activateSlowMotion = false;
    [SerializeField] private bool printDebugStates = false;
    [SerializeField] private string debugState = "";
    public bool ignoreCheckpoints = false;

    [Header("Config")]
    public PlayerConfig config;
    public Mechanics mechanics;
    public Shield shield;
    public GameObject platformPrefab;
    public GameObject bootsBulletPrefab;
    public GameObject normalSlash;
    public GameObject boostedSlash;
    public GameObject explosionPrefab;
    public GameObject cameraHolder;
    public GameObject cameraObj;
    [SerializeField] private Vector3 originalPosition = new Vector3(0, 0, 0);

    [Header("Particles")]
    public ParticleSystem walkParticles;
    public ParticleSystem leftSideParticles;
    public ParticleSystem rightSideParticles;
    public ParticleSystem dyingParticles;
    public ParticleSystem jumpParticles;

    [Header("Parameters")]
    public bool isGrounded;
    public bool isTouchingWall;
    public bool isTouchingRightWall;
    public bool isTouchingLeftWall;
    public bool isDying = false;
    public bool isParrying = false;

    public bool canDoubleJump;
    public bool canDash;
    public bool hasResetDashTrigger;

    public float dashCooldownTimer;
    public float attackCooldownTimer;
    public float blinkCooldownTimer;
    public float shieldCooldownTimer;
    public float explosionCooldownTimer;
    public float gunBootsCooldownTimer;
    public float walkParticlesCooldownTimer;

    public float coyoteTimer;
    public float bunnyHopTimer;
    public float parryTimer;
    public float airJumpInputBufferTimer;

    public static Vector3 respawnPosition;

    public Rigidbody2D rb { get; set; }
    public Animator animator { get; set; }
    public SpriteRenderer spriteRenderer { get; set; }

    public bool freezePlayerState { get; set; }
    public float moveSpeed { get; set; }
    public int lookingDirection { get; set; }
    public List<GameObject> keys { get; set; }

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        lookingDirection = 1;
        freezePlayerState = false;
        hasResetDashTrigger = true;

        keys = new List<GameObject>();
        Key.ResetAllSlots();
        SetMoveSpeed();

        IfDebugSetRespawnPosition();
        TransitionToState(GroundedState);
    }

    private void Update() {
        if (freezePlayerState) return;

        currentState.Update(this);

        IfDebugActivateSlowMotion();
        UpdateFacingSprite();
        PositionWalkParticles();
        if (!isDying) {
            CheckIfHasResetDashTrigger();
            shield.CheckShieldInput();
            CreatePlatform.CheckCreateInput(this);
            CreatePlatform.CheckDeleteInput(this);
        }
    }

    private void FixedUpdate() {
        if (freezePlayerState) return;

        ProcessTimers();
        currentState.FixedUpdate(this);
    }

    public void TransitionToState(PlayerBaseState state) {
        if (freezePlayerState) return;

        currentState = state;
        currentState.EnterState(this);

        IfDebugPrintStates();
    }

    private void UpdateFacingSprite() {
        if (lookingDirection == 1) {
            spriteRenderer.flipX = false;
        }
        else if (lookingDirection == -1) {
            spriteRenderer.flipX = true;
        }
    }

    private void PositionWalkParticles() {
        if (lookingDirection == 1) {
            walkParticles.transform.localPosition = new Vector3(-0.3f, -0.45f, 0);
            walkParticles.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (lookingDirection == -1) {
            walkParticles.transform.localPosition = new Vector3(0.3f, -0.45f, 0);
            walkParticles.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void ProcessTimers() {
        float step = Time.deltaTime;
        if (walkParticlesCooldownTimer >= 0) walkParticlesCooldownTimer -= step;
        if (airJumpInputBufferTimer >= 0) airJumpInputBufferTimer -= step;
        if (explosionCooldownTimer >= 0) explosionCooldownTimer -= step;
        if (gunBootsCooldownTimer >= 0) gunBootsCooldownTimer -= step;
        if (attackCooldownTimer >= 0) attackCooldownTimer -= step;
        if (shieldCooldownTimer >= 0) shieldCooldownTimer -= step;
        if (blinkCooldownTimer >= 0) blinkCooldownTimer -= step;
        if (dashCooldownTimer >= 0) dashCooldownTimer -= step;
        if (bunnyHopTimer >= 0) bunnyHopTimer -= step;
        if (coyoteTimer >= 0) coyoteTimer -= step;
        if (parryTimer >= 0) parryTimer -= step;
    }

    // To avoid dashing infinitely when holding trigger on the ground
    private void CheckIfHasResetDashTrigger() {
        if (Input.GetAxisRaw("Dash") == 0f) {
            hasResetDashTrigger = true;
        }
    }

    private void SetMoveSpeed() {
        moveSpeed = config.moveSpeed;
        if (mechanics.IsEnabled("Move Speed Boost")) {
            moveSpeed = config.moveSpeedBoosted;
        }
    }

    private void IfDebugSetRespawnPosition() {
        if (!ignoreCheckpoints) {
            if (respawnPosition == Vector3.zero) {
                respawnPosition = originalPosition;
            }
            transform.position = respawnPosition;
        }
    }

    private void IfDebugActivateSlowMotion() {
        if (canActivateSlowMotion) {
            if (activateSlowMotion) Time.timeScale = 0.2f;
            else Time.timeScale = 1f;
        }
    }

    private void IfDebugPrintStates() {
        if (printDebugStates) {
            debugState = currentState.GetType().Name;
            Debug.Log(debugState);
        }
    }
}