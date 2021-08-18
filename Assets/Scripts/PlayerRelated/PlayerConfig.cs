using UnityEngine;

[CreateAssetMenu()]
public class PlayerConfig : ScriptableObject {
    [Header("Mechanics Menu")]
    public int maxSkillPoints = 6;

    [Header("Camera Shake")]
    public float dyingShakeDuration = 0.15f;
    public float dyingShakeMagnitude = 0.4f;

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
    public float startAirJumpInputBufferTime = 0.2f;

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
    public float reflectedBulletsNeededToKill = 1;

    [Header("Explosion")]
    public float startExplosionCooldownTime = 0.5f;
    public float explosionRadius = 0.4f;
    public float explosionDamageRate = 0.5f;
    public float xRange = 1.2f;
    public float yRange = 1.3f;
    public float explosionShakeDuration = 0.15f;
    public float explosionShakeMagnitude = 0.4f;

    [Header("Gun Boots")]
    public float startGunBootsCooldownTime = 0.2f;
    public float bootsBulletSpeed = 6f;
    public float gunBootsForce = 3f;
    public float gunBootsMaxSpeed = 50f;
    public float gunBootsDamage = 1f;
}

