using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PlayerConfig : ScriptableObject {
    [Header("Walk")]
    public float moveSpeed = 6f;

    [Header("BetterFalling")]
    public float fallMultiplier = 3f;
    public float lowJumpMultiplier = 5f;

    [Header("Jump")]
    public float jumpForce = 12f;
    public float bunnyHopTime = 0;
    public float startBunnyHopTime = 0.15f;
    public float coyoteTime = 0;
    public float startCoyoteTime = 0.1f;

    [Header("DoubleJump")]
    public float doubleJumpForce = 12f;

    [Header("Dash")]
    public float dashSpeed = 23;
    public float startDashTime = 0.175f;
    public float startDashCooldownTime = 0.2f;

    [Header("WallSlide")]
    public float wallSlidingSpeed = 1.5f;
    public float startStickyTime = 0.1f;

    [Header("WallJump")]
    public float startWallJumpTime = 0.1f;
}

