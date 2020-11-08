using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : MonoBehaviour {
    private Player player;
    public float moveSpeed = 5f;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public Dash dashScript;

    [HideInInspector] public float xInput;
    [HideInInspector] public Rigidbody2D rb;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<Player>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        dashScript = FindObjectOfType<Dash>();
    }

    void Update() {
        xInput = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate() {
        if (player.disableControls) return;

        ProcessWalkAction();
    }

    void ProcessWalkAction() {
        if (dashScript && !dashScript.isDashing && !player.isWallJumping) {
            WalkAction(xInput);
        }
    }

    void WalkAction(float xInput) {
        int direction = 0;
        if (xInput > 0) {
            direction = 1;
        }
        else if (xInput < 0) {
            direction = -1;
        }

        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);

        animator.SetBool("isWalking", true);
        if (xInput < 0) {
            player.lastDirection = -1;
            player.spriteRenderer.flipX = true;
        }
        else if (xInput > 0) {
            player.lastDirection = 1;
            player.spriteRenderer.flipX = false;
        }
        else {
            animator.SetBool("isWalking", false);
        }
    }
}
