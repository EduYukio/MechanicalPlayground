using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PlayerConfig : ScriptableObject {
    [Header("Walk")]
    public float moveSpeed = 6f;

    [Header("Better Falling")]
    public float fallMultiplier = 3f;
    public float lowJumpMultiplier = 5f;
    public float maxFallSpeed = -15f;

    [Header("Jump")]
    public float jumpForce = 12f;
    public float startBunnyHopDurationTime = 0.15f;
    public float startCoyoteDurationTime = 0.1f;

    [Header("Double Jump")]
    public float doubleJumpForce = 12f;
    public float startAirJumpInputBuffer = 0.2f;

    [Header("Dash")]
    public float dashSpeed = 23;
    public float startDashDurationTime = 0.175f;
    public float startDashCooldownTime = 0.2f;
    public float etherealTransparency = 0.4f;

    [Header("Wall Slide")]
    public float wallSlidingSpeed = 1.5f;
    public float startStickyTime = 0.1f;

    [Header("Wall Jump")]
    public float startWallJumpDurationTime = 0.1f;

    [Header("Attack")]
    public float attackDamage = 10f;
    public float startAttackCooldownTime = 0.33f;
    public float pogoForce = 6f;

    [Header("Blink")]
    public float blinkDistance = 5f;
    public float blinkGroundCheckRadius = 0.2f;
    public float startPreBlinkTime = 0.2f;
    public float startPostBlinkTime = 0.2f;
    public float startBlinkCooldownTime = 0.2f;

    [Header("Move Speed Boost")]
    public float moveSpeedBoosted = 9f;

    [Header("Shield")]
    public float startShieldCooldownTime = 0.75f;
    public float startParryTime = 0.15f;
    public float parryPauseDuration = 0.25f;
}

