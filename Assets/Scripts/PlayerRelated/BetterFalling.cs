using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterFalling : MonoBehaviour {
    public float fallMultiplier = 3f;
    public float lowJumpMultiplier = 5f;

    // private Rigidbody2D rb;
    // private Animator animator;
    private PlayerFSM player;

    void Start() {
        // rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerFSM>();
        // animator = GetComponent<Animator>();
    }

    void Update() {
        BetterFallingAction();
    }

    void BetterFallingAction() {
        // if (player.isWallSliding) {
        // animator.SetBool("isFalling", false);
        // return;
        // }

        // animator.SetBool("isFalling", true);
        if (player.rb.velocity.y < 0) {
            player.rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (player.rb.velocity.y > 0 && !Input.GetButton("Jump")) {
            player.rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
        // else {
        // animator.SetBool("isFalling", false);
        // }
    }
}
