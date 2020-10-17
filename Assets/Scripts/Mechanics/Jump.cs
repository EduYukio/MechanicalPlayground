using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour {
    public float jumpForce = 12f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    private Player player;

    [HideInInspector] public Rigidbody2D rb;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<Player>();
    }

    void Update() {
        ProcessJumpRequest();
    }

    void ProcessJumpRequest() {
        BetterJumping();

        if (Input.GetButtonDown("Jump")) {
            if (player.isGrounded) {
                JumpAction();
            }
        }
    }

    void JumpAction() {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    void BetterJumping() {
        if (rb.velocity.y < 0) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump")) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
}
