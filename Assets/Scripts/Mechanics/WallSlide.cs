using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSlide : MonoBehaviour {
    public float wallSlidingSpeed = 1.5f;

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
        if (CanWallSlide()) {
            WallSlidingCheck(xInput);
        }

        StickynessCheck();

        if (player.isWallSliding) {
            WallSlideAction();
            ProcessStickyTime(xInput);
        }
    }

    void WallSlidingCheck(float xInput) {
        if (IsPressingAgainstTheWall(xInput)) {
            player.isWallSliding = true;
            animator.SetBool("isWallSliding", true);
            InvertPlayerDirection();
        }
    }

    bool CanWallSlide() {
        if (!player.isTouchingWall || player.isGrounded) {
            player.isWallSliding = false;
            stickyTime = startStickyTime;
            animator.SetBool("isWallSliding", false);
            return false;
        }

        return true;
    }

    void WallSlideAction() {
        float yVelocity = Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue);
        rb.velocity = new Vector2(rb.velocity.x, yVelocity);
    }

    void InvertPlayerDirection() {
        if (player.isTouchingLeftWall) {
            player.lastDirection = 1;
        }
        else if (player.isTouchingRightWall) {
            player.lastDirection = -1;
        }
    }

    void ProcessStickyTime(float xInput) {
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

    bool IsPressingAgainstTheWall(float xInput) {
        if (player.isTouchingLeftWall && xInput < 0) return true;
        if (player.isTouchingRightWall && xInput > 0) return true;

        return false;
    }

    void StickynessCheck() {
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
