using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : MonoBehaviour {
    private Player player;
    public float moveSpeed = 5f;

    [HideInInspector] public float xInput;
    [HideInInspector] public Rigidbody2D rb;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<Player>();
    }

    void Update() {
        xInput = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate() {
        if (player.disableControls) return;

        ProcessWalkAction();
    }

    void ProcessWalkAction() {
        // if (!isDashing) {
        WalkAction(xInput);
        // }
    }

    void WalkAction(float xInput) {
        int direction = 0;
        if (xInput > 0) {
            direction = 1;
        }
        else if (xInput < 0) {
            direction = -1;
        }
        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);

        // if (xInput < 0) {
        //     lastDirection = -1;
        //     spriteRenderer.flipX = true;
        // }
        // else if (xInput > 0) {
        //     lastDirection = 1;
        //     spriteRenderer.flipX = false;
        // }
    }
}
