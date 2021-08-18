using System.Collections.Generic;
using UnityEngine;

public class PlayerFSM : MonoBehaviour {
    [HideInInspector] public PlayerBaseState currentState;

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

    //DEBUG
    [Header("Debug")]
    public bool ignoreConfirmationPopup = false;
    public bool ignoreCheckpoints = false;
    public bool canActivateSlowMotion = false;
    public bool activateSlowMotion = false;
    public bool printDebugStates = false;
    public string debugState;
    //DEBUG

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
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator animator;
    [HideInInspector] public SpriteRenderer spriteRenderer;

    [Header("Particles")]
    public ParticleSystem walkParticles;
    public ParticleSystem leftSideParticles;
    public ParticleSystem rightSideParticles;
    public ParticleSystem jumpParticles;
    public ParticleSystem dyingParticles;

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
    public float airJumpInputBuffer;

    [HideInInspector] public float moveSpeed;
    [HideInInspector] public int lastDirection = 1;
    public Transform centerTransform;
    public static Vector3 respawnPosition;
    public Vector3 originalPosition = new Vector3(0, 0, 0);
    public bool freezePlayerState = false;
    public List<GameObject> keys;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        freezePlayerState = false;
        hasResetDashTrigger = true;
        keys = new List<GameObject>();
        Key.ResetAllSlots();
        SetMoveSpeed();

        //DEBUG
        if (!ignoreCheckpoints) {
            if (respawnPosition == Vector3.zero) {
                respawnPosition = originalPosition;
            }
            transform.position = respawnPosition;
        }
        //DEBUG

        TransitionToState(GroundedState);
    }

    private void Update() {
        if (freezePlayerState) return;

        currentState.Update(this);

        //DEBUG
        if (canActivateSlowMotion) {
            if (activateSlowMotion) Time.timeScale = 0.2f;
            else Time.timeScale = 1f;
        }
        //DEBUG

        UpdateFacingSprite();
        PositionWalkParticles();
        ProcessTimers();
        if (!isDying) {
            CheckIfHasResetDashTrigger();
            shield.CheckShieldInput();
            CreatePlatform.CheckCreateInput(this);
            CreatePlatform.CheckDeleteInput(this);
        }
    }

    private void FixedUpdate() {
        if (freezePlayerState) return;

        currentState.FixedUpdate(this);
    }

    public void TransitionToState(PlayerBaseState state) {
        if (freezePlayerState) return;

        currentState = state;
        currentState.EnterState(this);

        //DEBUG
        if (printDebugStates) {
            debugState = currentState.GetType().Name;
            Debug.Log(debugState);
        }
        //DEBUG
    }


    // Helper functions

    private void UpdateFacingSprite() {
        if (lastDirection == 1) {
            spriteRenderer.flipX = false;
        }
        else if (lastDirection == -1) {
            spriteRenderer.flipX = true;
        }
    }

    private void PositionWalkParticles() {
        if (lastDirection == 1) {
            walkParticles.transform.localPosition = new Vector3(-0.3f, -0.45f, 0);
            walkParticles.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (lastDirection == -1) {
            walkParticles.transform.localPosition = new Vector3(0.3f, -0.45f, 0);
            walkParticles.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void ProcessTimers() {
        float step = Time.deltaTime;
        if (walkParticlesCooldownTimer >= 0) walkParticlesCooldownTimer -= step;
        if (gunBootsCooldownTimer >= 0) gunBootsCooldownTimer -= step;
        if (explosionCooldownTimer >= 0) explosionCooldownTimer -= step;
        if (attackCooldownTimer >= 0) attackCooldownTimer -= step;
        if (shieldCooldownTimer >= 0) shieldCooldownTimer -= step;
        if (blinkCooldownTimer >= 0) blinkCooldownTimer -= step;
        if (airJumpInputBuffer >= 0) airJumpInputBuffer -= step;
        if (dashCooldownTimer >= 0) dashCooldownTimer -= step;
        if (bunnyHopTimer >= 0) bunnyHopTimer -= step;
        if (coyoteTimer >= 0) coyoteTimer -= step;
        if (parryTimer >= 0) parryTimer -= step;
    }

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
}