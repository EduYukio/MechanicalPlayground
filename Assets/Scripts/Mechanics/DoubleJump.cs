using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : MonoBehaviour {
    public float jumpForce = 12f;
    private Animator animator;
    private Player player;
    private Rigidbody2D rb;
    private bool alreadyDoubleJumped = false;

    void Start() {
        player = FindObjectOfType<Player>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update() {
        CheckIfCanDoubleJump();
        ProcessDoubleJumpRequest();
    }

    void ProcessDoubleJumpRequest() {
        if (!player.nextJumpIsDouble) return;
        if (alreadyDoubleJumped) return;
        if (player.isWallSliding) return;

        if (Input.GetButtonDown("Jump")) {
            DoubleJumpAction();
        }
    }

    void DoubleJumpAction() {
        alreadyDoubleJumped = true;
        animator.SetTrigger("DoubleJump");
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    void CheckIfCanDoubleJump() {
        if (player.isGrounded) {
            player.nextJumpIsDouble = false;
            alreadyDoubleJumped = false;
        }
        else if (player.isWallSliding) {
            player.nextJumpIsDouble = true;
            alreadyDoubleJumped = false;
        }
        else if (Input.GetButtonUp("Jump") && !alreadyDoubleJumped) {
            player.nextJumpIsDouble = true;
        }
    }
}
