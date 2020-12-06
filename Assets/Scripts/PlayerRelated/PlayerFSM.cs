using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

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

    public PlayerConfig config;
    public Mechanics mechanics;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator animator;
    [HideInInspector] public SpriteRenderer spriteRenderer;

    public bool isGrounded;
    public bool isTouchingWall;
    public bool isTouchingRightWall;
    public bool isTouchingLeftWall;

    public bool canDoubleJump;
    public bool canDash;
    public bool hasResetDashTrigger;
    public float dashCooldownTimer;
    public float attackCooldownTimer;
    public float blinkCooldownTimer;
    public float coyoteTimer;
    public float bunnyHopTimer;
    public float airJumpInputBuffer;


    public float moveSpeed;
    public static Vector3 respawnPosition;
    public Vector3 originalPosition = new Vector3(0, 0, 0);
    public bool freezePlayerState = false;
    public int lastDirection = 1;
    public int items;

    //DEBUG
    public bool debugMode = false;
    public bool activateSlowMotion = false;
    public bool printDebugStates = false;
    public string debugState;
    //DEBUG

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        freezePlayerState = false;
        hasResetDashTrigger = true;

        if (!debugMode) {
            if (respawnPosition == Vector3.zero) {
                respawnPosition = originalPosition;
            }
            transform.position = respawnPosition;
        }

        items = 0;

        TransitionToState(GroundedState);
    }

    private void Update() {
        if (freezePlayerState) return;

        UpdateMoveSpeed();
        ProcessTimers();
        CheckIfHasResetDashTrigger();

        currentState.Update(this);

        UpdateFacingSprite();

        //DEBUG
        if (debugMode) {
            if (activateSlowMotion) Time.timeScale = 0.2f;
            else Time.timeScale = 1f;
        }
        //DEBUG
    }

    public void TransitionToState(PlayerBaseState state) {
        if (freezePlayerState) return;

        currentState = state;
        currentState.EnterState(this);

        //DEBUG
        debugState = currentState.GetType().Name;
        if (printDebugStates) Debug.Log(debugState);
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

    private void ProcessTimers() {
        float step = Time.deltaTime;
        if (dashCooldownTimer >= 0) dashCooldownTimer -= step;
        if (attackCooldownTimer >= 0) attackCooldownTimer -= step;
        if (blinkCooldownTimer >= 0) blinkCooldownTimer -= step;
        if (coyoteTimer >= 0) coyoteTimer -= step;
        if (bunnyHopTimer >= 0) bunnyHopTimer -= step;
        if (airJumpInputBuffer >= 0) airJumpInputBuffer -= step;
    }

    void CheckIfHasResetDashTrigger() {
        if (Input.GetAxisRaw("Dash") == 0f) {
            hasResetDashTrigger = true;
        }
    }

    // quando for lançar, cachear a movespeed do player ao inves de dar update todo frame
    void UpdateMoveSpeed() {
        moveSpeed = config.moveSpeed;
        if (mechanics.IsEnabled("Move Speed Boost")) {
            moveSpeed = config.moveSpeedBoosted;
        }
    }
}