using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour {
    public Rigidbody2D rb;
    public CollisionChecker coll;
    public float fallMultiplier = 3f;
    public float lowJumpMultiplier = 15f;
    public float jumpForce = 15f;

    public int jumpQuantity;

    void Start() {
        coll = GetComponent<CollisionChecker>();
        rb = GetComponent<Rigidbody2D>();
        jumpQuantity = 0;
    }

    void Update() {
        if (coll.onGround) {
            jumpQuantity = 0;
        }

        if (Input.GetButtonDown("Jump") && jumpQuantity == 0) {
            JumpAction(Vector2.up, false);
        }

        if (Input.GetButtonUp("Jump")) {
            jumpQuantity++;
        }

        SmoothJump();
        rb.gravityScale = 3;
    }

    public void JumpAction(Vector2 dir, bool wall) {
        //rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity = dir * jumpForce;
    }

    private void SmoothJump() {
        if (rb.velocity.y < 0) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump")) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
}
