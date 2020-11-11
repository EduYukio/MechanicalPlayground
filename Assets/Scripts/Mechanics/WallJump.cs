using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJump : MonoBehaviour {
    public float xWallForce;
    public float yWallForce;
    public float wallJumpTime;
    private Rigidbody2D rb;
    private Player player;
    private Animator animator;
    int jumpDirection = 0;

    void Start() {
        player = FindObjectOfType<Player>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update() {
        ProcessWallJumpRequest();

        if (player.isWallSliding) {
            CheckWallJumpDirection();
        }

        if (player.isWallJumping) {
            WallJumpAction();
        }
    }

    void ProcessWallJumpRequest() {
        if (!player.isWallSliding) return;

        if (Input.GetButtonDown("Jump")) {
            player.isWallJumping = true;
            Invoke(nameof(SetWallJumpingToFalse), wallJumpTime);
        }
    }

    void WallJumpAction() {
        rb.velocity = new Vector2(xWallForce * jumpDirection, yWallForce);
        animator.SetTrigger("Jump");
    }

    void SetWallJumpingToFalse() {
        player.isWallJumping = false;
    }

    void CheckWallJumpDirection() {
        if (player.isTouchingLeftWall) {
            jumpDirection = 1;
        }
        else {
            jumpDirection = -1;
        }
    }
}
