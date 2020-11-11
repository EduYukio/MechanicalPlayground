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
        WallSlideAction();
    }

    void CheckWallSliding() {
        float xInput = Input.GetAxisRaw("Horizontal");
        if (player.isTouchingWall && !player.isGrounded && xInput != 0) {
            player.isWallSliding = true;
            animator.SetBool("isWallSliding", true);
        }
        else {
            player.isWallSliding = false;
            animator.SetBool("isWallSliding", false);
        }
    }

    void WallSlideAction() {
        if (player.isWallSliding) {
            float yVelocity = Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue);
            rb.velocity = new Vector2(rb.velocity.x, yVelocity);
        }
    }
}
