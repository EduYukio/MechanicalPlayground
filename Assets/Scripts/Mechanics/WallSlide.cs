using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSlide : MonoBehaviour {
    public float wallSlidingSpeed;
    private Rigidbody2D rb;
    private Player player;
    private Animator animator;

    void Start() {
        player = FindObjectOfType<Player>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update() {
        CheckWallSliding();

        if (player.isWallSliding) {
            WallSlideAction();
        }
    }

    void CheckWallSliding() {
        if (!player.isTouchingWall || player.isGrounded) {
            player.isWallSliding = false;
            animator.SetBool("isWallSliding", false);
            return;
        }

        FixPlayerSpriteWhenSliding();
        float xInput = Input.GetAxisRaw("Horizontal");
        if (player.isTouchingLeftWall && xInput < 0) {
            // facing left wall
            player.isWallSliding = true;
            animator.SetBool("isWallSliding", true);
        }
        else if (player.isTouchingRightWall && xInput > 0) {
            // facing right wall
            player.isWallSliding = true;
            animator.SetBool("isWallSliding", true);
        }
    }

    void WallSlideAction() {
        float yVelocity = Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue);
        rb.velocity = new Vector2(rb.velocity.x, yVelocity);
    }

    void FixPlayerSpriteWhenSliding() {
        if (player.isTouchingLeftWall) {
            player.lastDirection = -1;
        }
        else if (player.isTouchingRightWall) {
            player.lastDirection = 1;
        }
    }
}
