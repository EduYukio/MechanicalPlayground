using System.Collections;
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

    public PlayerConfig config;
    public Mechanics mechanics;
    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    public bool isGrounded;
    public bool isTouchingWall;
    public bool isTouchingRightWall;
    public bool isTouchingLeftWall;

    public bool canDoubleJump;
    public bool canDash;
    public float dashCooldownTimer;
    public float attackCooldownTimer;
    public float coyoteTimer;
    public float bunnyHopTimer;
    public int lastDirection = 1;

    //DEBUG
    public string debugState;
    public bool activateSlowMotion = false;
    public bool printDebugStates = false;
    //DEBUG

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        TransitionToState(GroundedState);
    }

    private void Update() {
        ProcessTimers();

        currentState.Update(this);

        UpdateFacingSprite();

        //DEBUG
        DebugSlowMotion();
        //DEBUG
    }

    public void TransitionToState(PlayerBaseState state) {
        currentState = state;
        currentState.EnterState(this);

        //DEBUG
        debugState = currentState.GetType().Name;
        if (printDebugStates) Debug.Log(debugState);
        //DEBUG
    }

    private void UpdateFacingSprite() {
        if (lastDirection == 1) {
            spriteRenderer.flipX = false;
        }
        else if (lastDirection == -1) {
            spriteRenderer.flipX = true;
        }
    }

    void ProcessTimers() {
        float step = Time.deltaTime;
        if (dashCooldownTimer >= 0) dashCooldownTimer -= step;
        if (attackCooldownTimer >= 0) attackCooldownTimer -= step;
        if (coyoteTimer >= 0) coyoteTimer -= step;
        if (bunnyHopTimer >= 0) bunnyHopTimer -= step;
    }

    //DEBUG
    void DebugSlowMotion() {
        if (activateSlowMotion) {
            Time.timeScale = 0.2f;
        }
        else {
            Time.timeScale = 1f;
        }
    }
    //DEBUG
}