using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSlide : MonoBehaviour {
    bool isWallSliding = false;
    public float wallSlidingSpeed;
    Player player;
    private Rigidbody2D rb;

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
            isWallSliding = true;
        }
        else {
            isWallSliding = false;
        }
    }

    void WallSlideAction() {
        if (isWallSliding) {
            float yVelocity = Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue);
            rb.velocity = new Vector3(rb.velocity.x, yVelocity);
        }
    }
}
