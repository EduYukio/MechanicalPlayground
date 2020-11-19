using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFSM : MonoBehaviour {
    private PlayerBaseState currentState;

    public readonly PlayerGroundedState GroundedState = new PlayerGroundedState();
    public readonly PlayerJumpingState JumpingState = new PlayerJumpingState();
    public readonly PlayerFallingState FallingState = new PlayerFallingState();
    public readonly PlayerWalkingState WalkingState = new PlayerWalkingState();
    public readonly PlayerDoubleJumpingState DoubleJumpingState = new PlayerDoubleJumpingState();
    public readonly PlayerDashingState DashingState = new PlayerDashingState();

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
    public int lastDirection = 1;

    //DEBUG
    public string debugState;
    //DEBUG

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        TransitionToState(GroundedState);
    }

    private void Update() {
        ProcessDashCooldown();

        currentState.Update(this);

        UpdateFacingSprite();
    }

    public void TransitionToState(PlayerBaseState state) {
        currentState = state;
        currentState.EnterState(this);

        //DEBUG
        debugState = currentState.GetType().Name;
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

    void ProcessDashCooldown() {
        if (dashCooldownTimer > 0) {
            dashCooldownTimer -= Time.deltaTime;
        }
    }
}