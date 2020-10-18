using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour {
    public float dashSpeed = 30;

    public float startDashTime = 0.2f;
    [HideInInspector] public float dashTime;

    public float startDashCooldownTime = 0.5f;
    [HideInInspector] public float dashCooldownTime;

    public bool canDash = true;
    public bool isDashing = false;

    [HideInInspector] public Rigidbody2D rb;
    private Player player;

    void Start() {
        player = FindObjectOfType<Player>();
        rb = GetComponent<Rigidbody2D>();

        dashTime = startDashTime;
        dashCooldownTime = 0f;
    }

    void Update() {
        if (player.isGrounded) {
            canDash = true;
        }

        ProcessDashCooldown();
        ProcessDashRequest();
        DashAction();
    }

    void ProcessDashRequest() {
        if (Input.GetButtonDown("Dash") && canDash && !isDashing && dashCooldownTime <= 0) {
            isDashing = true;
        }
    }

    void DashAction() {
        if (isDashing) {
            if (dashTime > 0) {
                dashTime -= Time.deltaTime;
                rb.velocity = new Vector2(player.lastDirection * dashSpeed, 0f);
            }
            else {
                StopDashing();
            }
        }
    }

    void ProcessDashCooldown() {
        if (dashCooldownTime > 0) {
            dashCooldownTime -= Time.deltaTime;
        }
    }

    public void StopDashing() {
        rb.velocity = Vector2.zero;
        isDashing = false;
        canDash = false;
        dashCooldownTime = startDashCooldownTime;
        dashTime = startDashTime;
    }
}
