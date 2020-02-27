using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour {
    public Rigidbody2D rb;
    public CollisionChecker coll;
    public float jumpForce = 15f;
    public int jumpQuantity = 0;

    void Start() {
        coll = GetComponent<CollisionChecker>();
        rb = GetComponent<Rigidbody2D>();
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
    }

    public void JumpAction(Vector2 dir, bool wall) {
        //rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity = dir * jumpForce;
    }
}
