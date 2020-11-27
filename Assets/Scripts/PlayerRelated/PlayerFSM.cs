using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public float dashCooldownTimer;
    public float attackCooldownTimer;
    public float blinkCooldownTimer;
    public float coyoteTimer;
    public float bunnyHopTimer;

    public int lastDirection = 1;
    public float moveSpeed;
    public int items;

    //DEBUG
    public string debugState;
    public bool activateSlowMotion = false;
    public bool printDebugStates = false;
    //DEBUG

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        moveSpeed = config.moveSpeed;
        if (mechanics.IsEnabled("MoveSpeedBoost")) {
            moveSpeed = config.moveSpeedBoosted;
        }

        items = 0;

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

    public void Die() {
        //play die animation
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
    }

    //DEBUG
    private void DebugSlowMotion() {
        if (activateSlowMotion) {
            Time.timeScale = 0.2f;
        }
        else {
            Time.timeScale = 1f;
        }
    }
    //DEBUG
}