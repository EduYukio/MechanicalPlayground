using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour {
    public float jumpForce = 12f;
    [SerializeField] private float bunnyHopTime = 0;
    [SerializeField] private float startBunnyHopTime = 0.2f;
    [SerializeField] private float coyoteTime = 0;
    [SerializeField] private float startCoyoteTime = 0.05f;
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
        CoyoteTimeCheck();
        ProcessJumpRequest();
    }

    void ProcessJumpRequest() {
        if (player.disableControls) return;
        if (bunnyHopTime <= 0) return;
        if (coyoteTime <= 0) return;
        if (player.isWallSliding) return;
        if (player.nextJumpIsDouble) return;

        JumpAction();
    }

    void JumpAction() {
        bunnyHopTime = 0;
        coyoteTime = 0;
        animator.SetTrigger("Jump");
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    void BunnyHopCheck() {
        if (bunnyHopTime >= 0) bunnyHopTime -= Time.deltaTime;

        if (Input.GetButtonDown("Jump")) {
            bunnyHopTime = startBunnyHopTime;
        }
    }

    void CoyoteTimeCheck() {
        if (coyoteTime >= 0) coyoteTime -= Time.deltaTime;

        if (player.isGrounded) {
            coyoteTime = startCoyoteTime;
        }
    }
}
