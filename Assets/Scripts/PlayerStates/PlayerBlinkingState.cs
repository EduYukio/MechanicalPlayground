using UnityEngine;

public class PlayerBlinkingState : PlayerBaseState {
    float beginBlinkTimer;
    float endBlinkTimer;
    float originalGravity;
    bool alreadyBlinked;

    public override void EnterState(PlayerFSM player) {
        player.animator.Play("PlayerDisappear");
        Setup(player);
    }

    public override void Update(PlayerFSM player) {
        if (beginBlinkTimer > 0) {
            beginBlinkTimer -= Time.deltaTime;
            return;
        }

        if (!alreadyBlinked) {
            player.animator.Play("PlayerAppear");
            BlinkAction(player);
            alreadyBlinked = true;
        }

        if (endBlinkTimer > 0) {
            endBlinkTimer -= Time.deltaTime;
            return;
        }

        StopBlinking(player);

        if (base.CheckTransitionToGrounded(player)) return;
        if (base.CheckTransitionToFalling(player)) return;
    }

    void Setup(PlayerFSM player) {
        alreadyBlinked = false;
        beginBlinkTimer = player.config.startPreBlinkTime;
        endBlinkTimer = player.config.startPostBlinkTime;
        player.rb.velocity = Vector2.zero;
        originalGravity = player.rb.gravityScale;
        player.rb.gravityScale = 0f;
    }

    void BlinkAction(PlayerFSM player) {
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        Vector3 blinkDirection;
        if (xInput == 0 && yInput == 0) {
            blinkDirection = new Vector3(player.lastDirection, 0f, 0f);
        }
        else {
            blinkDirection = new Vector3(xInput, yInput, 0f);
        }

        Vector3 newPos = ValidDestinationPosition(player, blinkDirection);
        player.transform.Translate(newPos);
    }

    Vector3 ValidDestinationPosition(PlayerFSM player, Vector3 blinkDirection) {
        float distance = player.config.blinkDistance;
        Vector3 finalPosition = blinkDirection * distance;
        LayerMask groundLayer = LayerMask.GetMask("Ground");
        float radius = 0.3f;
        float step = 0.1f;

        Collider2D[] destinationColliders = Physics2D.OverlapCircleAll(player.transform.localPosition + finalPosition, radius, groundLayer);
        while (destinationColliders.Length > 0) {
            distance = distance - step;
            finalPosition = blinkDirection * distance;
            destinationColliders = Physics2D.OverlapCircleAll(player.transform.localPosition + finalPosition, radius, groundLayer);
        }

        return finalPosition;
    }

    void StopBlinking(PlayerFSM player) {
        player.rb.gravityScale = originalGravity;
        player.rb.velocity = Vector2.zero;
        player.blinkCooldownTimer = player.config.startBlinkCooldownTime;
    }
}
