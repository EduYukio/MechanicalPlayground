using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : MonoBehaviour {
    public float jumpForce = 12f;
    public bool canDoubleJump = true;
    public Animator animator;

    private Player player;
    [HideInInspector] public Rigidbody2D rb;

    void Start() {
        player = FindObjectOfType<Player>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update() {
        if (player.isGrounded) {
            canDoubleJump = true;
        }

        if (Input.GetButtonDown("Jump")) {
            if (!player.isGrounded && canDoubleJump) {
                DoubleJumpAction();
                canDoubleJump = false;
            }
        }
    }

    void DoubleJumpAction() {
        animator.SetTrigger("DoubleJump");
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
}
