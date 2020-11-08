using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSlide : MonoBehaviour {
    public float wallSlidingSpeed;
    private Rigidbody2D rb;
    Player player;

    void Start() {
        player = GameObject.Find("Player").GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        checkIfWallSliding();
        WallSlideAction();
    }

    void checkIfWallSliding() {
        float xInput = Input.GetAxisRaw("Horizontal");
        if (player.isTouchingWall && !player.isGrounded && xInput != 0) {
            player.isWallSliding = true;
        }
        else {
            player.isWallSliding = false;
        }
    }

    void WallSlideAction() {
        if (player.isWallSliding) {
            float yVelocity = Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue);
            rb.velocity = new Vector2(rb.velocity.x, yVelocity);
        }
    }
}
