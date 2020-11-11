using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : MonoBehaviour {
    public float moveSpeed = 5f;
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    private Player player;
    private Rigidbody2D rb;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<Player>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update() {
        ProcessWalkRequest();
    }

    // *Checar depois se devo utilizar fixed update ou colocar time.deltaTime*
    // private void FixedUpdate() {
    // }

    void ProcessWalkRequest() {
        float xInput = Input.GetAxisRaw("Horizontal");
        int direction = CalculateDirection(xInput);
        SetWalkAnimation(direction);

        if (player.disableControls) return;
        if (player.isDashing) return;
        if (player.isWallJumping) return;
        if (player.isGluedOnTheWall) return;

        WalkAction(direction);
    }

    void WalkAction(float direction) {
        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);
    }

    int CalculateDirection(float xInput) {
        if (xInput > 0) {
            return 1;
        }
        else if (xInput < 0) {
            return -1;
        }
        else return 0;
    }

    void SetWalkAnimation(int direction) {
        if (direction == 0 || player.isWallSliding) {
            animator.SetBool("isWalking", false);
        }
        else if (!player.isTouchingWall) {
            player.lastDirection = direction;
            animator.SetBool("isWalking", true);
        }
    }
}
