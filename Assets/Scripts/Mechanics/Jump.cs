using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour {
    public float jumpForce = 12f;
    private Player player;
    private Animator animator;
    private Rigidbody2D rb;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<Player>();
        animator = GetComponent<Animator>();
    }

    void Update() {
        ProcessJumpRequest();
    }

    void ProcessJumpRequest() {
        if (Input.GetButtonDown("Jump")) {
            if (player.isGrounded && !player.isWallSliding) {
                JumpAction();
            }
        }
    }

    void JumpAction() {
        animator.SetTrigger("Jump");
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
}
