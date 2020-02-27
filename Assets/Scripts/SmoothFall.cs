using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFall : MonoBehaviour {
    public Rigidbody2D rb;
    public float fallMultiplier = 3f;
    public float lowJumpMultiplier = 15f;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        rb.gravityScale = 3;

        if (rb.velocity.y < 0) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump")) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
}
