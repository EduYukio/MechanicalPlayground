using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSlide : MonoBehaviour {
    public float wallSlidingSpeed;
    private Rigidbody2D rb;
    private Player player;
    private Animator animator;
    [SerializeField] private float stickyTime = 0;
    [SerializeField] private float startStickyTime = 0.1f;

    void Start() {
        player = FindObjectOfType<Player>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update() {
        float xInput = Input.GetAxisRaw("Horizontal");
        CheckWallSliding(xInput);
        CheckStickyness();

        if (player.isWallSliding) {
            WallSlideAction();
            WallStickyness(xInput);
        }
    }

    void CheckWallSliding(float xInput) {
        if (player.isWallSliding) {
            FixPlayerSpriteWhenSliding();
        }
        if (!player.isTouchingWall || player.isGrounded) {
            player.isWallSliding = false;
            stickyTime = startStickyTime;
            animator.SetBool("isWallSliding", false);
            // FixPlayerSpriteWhenSliding();
            return;
        }

        // FixPlayerSpriteWhenSliding();
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

    void WallStickyness(float xInput) {
        if (IsSlidingWithoutMoveInput(xInput)) {
            stickyTime = startStickyTime;
            return;
        }

        if (stickyTime >= 0) stickyTime -= Time.deltaTime;
    }

    bool IsSlidingWithoutMoveInput(float xInput) {
        if (xInput == 0) return true;
        if (xInput < 0 && player.isTouchingLeftWall) return true;
        if (xInput > 0 && player.isTouchingRightWall) return true;

        return false;
    }

    void CheckStickyness() {
        if (!player.isWallSliding) {
            player.isGluedOnTheWall = false;
            return;
        }

        if (stickyTime > 0) {
            player.isGluedOnTheWall = true;
        }
        else {
            player.isGluedOnTheWall = false;
        }
    }
}
