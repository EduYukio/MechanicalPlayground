using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJump : MonoBehaviour {
    public float xWallForce;
    public float yWallForce;
    public float wallJumpTime;
    private Rigidbody2D rb;
    Player player;

    void Start() {
        player = FindObjectOfType<Player>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        ProcessWallJumpRequest();
        WallJumpAction();
    }

    void ProcessWallJumpRequest() {
        if (Input.GetButtonDown("Jump") && player.isWallSliding) {
            player.isWallJumping = true;
            Invoke(nameof(SetWallJumpingToFalse), wallJumpTime);
        }
    }

    void WallJumpAction() {
        float xInput = Input.GetAxisRaw("Horizontal");
        if (player.isWallJumping) {
            rb.velocity = new Vector2(xWallForce * -xInput, yWallForce);
        }
    }

    void SetWallJumpingToFalse() {
        player.isWallJumping = false;
    }
}
