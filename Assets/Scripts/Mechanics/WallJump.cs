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

    void Start() {
        player = FindObjectOfType<Player>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update() {
        ProcessWallJumpRequest();

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
        float xInput = Input.GetAxisRaw("Horizontal");
        animator.SetTrigger("Jump");
        rb.velocity = new Vector2(xWallForce * -xInput, yWallForce);
    }

    void SetWallJumpingToFalse() {
        player.isWallJumping = false;
    }
}
