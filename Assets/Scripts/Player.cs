using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public bool isGrounded = false;
    public bool isTouchingWall = false;
    public bool isWallSliding = false;
    public bool isWallJumping = false;
    public bool isDashing = false;
    public bool nextJumpIsDouble = false;
    [HideInInspector] public bool disableControls = false;
    [HideInInspector] public int lastDirection = 1;

    void Start() {
    }

    void Update() {
    }
}
