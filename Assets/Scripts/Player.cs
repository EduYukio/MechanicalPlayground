using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public bool isGrounded = false;
    public bool disableControls = false;
    public bool canDoubleJump = true;

    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public float xInput;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        if (disableControls) return;

        if (isGrounded) {
            canDoubleJump = true;
        }

        xInput = Input.GetAxisRaw("Horizontal");
        ProcessJumpAction();
    }

    private void FixedUpdate() {
        if (disableControls) return;

        ProcessWalkAction();
    }

    void ProcessWalkAction() {
        // if (!isDashing) {
        Walk(xInput);
        // }
    }

    void Walk(float xInput) {
        int direction = 0;
        if (xInput > 0) {
            direction = 1;
        }
        else if (xInput < 0) {
            direction = -1;
        }
        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);

        // if (xInput < 0) {
        //     lastDirection = -1;
        //     spriteRenderer.flipX = true;
        // }
        // else if (xInput > 0) {
        //     lastDirection = 1;
        //     spriteRenderer.flipX = false;
        // }
    }

    void ProcessJumpAction() {
        if (Input.GetButtonDown("Jump")) {
            if (isGrounded) {
                Jump();
            }
            else if (canDoubleJump) {
                Jump();
                canDoubleJump = false;
            }
        }
    }

    void Jump() {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
}
