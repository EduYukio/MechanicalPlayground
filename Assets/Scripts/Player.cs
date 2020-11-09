using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public bool isGrounded = false;
    public bool isTouchingWall = false;
    public bool isWallSliding = false;
    public bool isWallJumping = false;
    public bool nextJumpIsDouble = false;
    [HideInInspector] public bool disableControls = false;
    [HideInInspector] public int lastDirection = 1;
    [HideInInspector] public SpriteRenderer spriteRenderer;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update() {
        if (disableControls) return;
    }
}
