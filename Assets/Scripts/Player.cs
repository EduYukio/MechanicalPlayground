using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public bool isGrounded = false;
    public bool isTouchingWall = false;
    public bool isTouchingRightWall;
    public bool isTouchingLeftWall;
    public bool isWallSliding = false;
    public bool isWallJumping = false;
    public bool isDashing = false;
    public bool nextJumpIsDouble = false;
    public bool activateSlowMotion = false;
    [HideInInspector] public bool disableControls = false;
    [HideInInspector] public int lastDirection = 1;

    private SpriteRenderer spriteRenderer;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        // FOR DEBUG PURPOSES, REMOVE WHEN LAUNCH GAME
        DebugSlowMotion();

        UpdateFacingSprite();
    }

    void DebugSlowMotion() {
        if (activateSlowMotion) {
            Time.timeScale = 0.2f;
        }
        else {
            Time.timeScale = 1f;
        }
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
