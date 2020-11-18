using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFSM : MonoBehaviour {
    private PlayerBaseState currentState;

    //DEBUGGGG
    public string debugState;
    //DEBUGGGG

    public readonly PlayerGroundedState GroundedState = new PlayerGroundedState();
    public readonly PlayerJumpingState JumpingState = new PlayerJumpingState();
    public readonly PlayerFallingState FallingState = new PlayerFallingState();
    public readonly PlayerWalkingState WalkingState = new PlayerWalkingState();

    public PlayerConfig config;
    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    public bool isGrounded;
    public bool isTouchingWall;
    public bool isTouchingRightWall;
    public bool isTouchingLeftWall;
    public int lastDirection = 1;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        TransitionToState(GroundedState);
    }

    private void Update() {
        currentState.Update(this);

        UpdateFacingSprite();
    }

    public void TransitionToState(PlayerBaseState state) {
        currentState = state;
        currentState.EnterState(this);

        //DEBUGG
        debugState = currentState.GetType().Name;
        //DEBUGG
    }

    private void UpdateFacingSprite() {
        if (lastDirection == 1) {
            spriteRenderer.flipX = false;
        }
        else if (lastDirection == -1) {
            spriteRenderer.flipX = true;
        }
    }
}