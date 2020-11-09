using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour {
    public float jumpForce = 12f;
    [SerializeField] private float bunnyHopTimer = 0;
    [SerializeField] private float startBunnyHopTime = 0.2f;
    private Player player;
    private Animator animator;
    private Rigidbody2D rb;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<Player>();
        animator = GetComponent<Animator>();
    }

    void Update() {
        BunnyHopCheck();
        ProcessJumpRequest();
    }

    void ProcessJumpRequest() {
        if (bunnyHopTimer <= 0) return;
        if (!player.isGrounded) return;
        if (player.isWallSliding) return;

        JumpAction();
    }

    void JumpAction() {
        bunnyHopTimer = 0;
        animator.SetTrigger("Jump");
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }


    void BunnyHopCheck() {
        if (bunnyHopTimer >= 0) bunnyHopTimer -= Time.deltaTime;

        if (Input.GetButtonDown("Jump")) {
            bunnyHopTimer = startBunnyHopTime;
        }
    }
}
