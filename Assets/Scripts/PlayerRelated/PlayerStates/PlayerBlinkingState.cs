using UnityEngine;

public class PlayerBlinkingState : PlayerBaseState {
    private float beginBlinkTimer;
    private float endBlinkTimer;
    private float originalGravity;
    private bool alreadyBlinked;
    private float xInput;
    private float yInput;

    public override void EnterState(PlayerFSM player) {
        player.animator.SetFloat("disappearSpeedMultiplier", 2.2f);
        player.animator.Play("PlayerDisappear");
        Manager.audio.Play("Blink");
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

        if (base.CheckTransitionToGunBoots(player)) return;
        if (base.CheckTransitionToGrounded(player)) return;
        if (base.CheckTransitionToFalling(player)) return;
    }

    private void Setup(PlayerFSM player) {
        alreadyBlinked = false;
        beginBlinkTimer = player.config.startPreBlinkTime;
        endBlinkTimer = player.config.startPostBlinkTime;
        player.rb.velocity = Vector2.zero;
        originalGravity = player.rb.gravityScale;
        player.rb.gravityScale = 0f;
        InputBuffer();
    }

    private void BlinkAction(PlayerFSM player) {
        Vector3 blinkDirection = base.GetFourDirectionalInput(player, xInput, yInput);
        Vector3 newPos = ValidDestinationPosition(player, blinkDirection);
        player.transform.Translate(newPos);
    }

    private Vector3 ValidDestinationPosition(PlayerFSM player, Vector3 blinkDirection) {
        float distance = player.config.blinkDistance;
        Vector3 finalPosition = blinkDirection * distance;
        LayerMask invalidLayers = LayerMask.GetMask("Ground", "Gate");
        float radius = player.config.blinkGroundCheckRadius;
        float step = 0.1f;

        Collider2D[] destinationColliders = Physics2D.OverlapCircleAll(player.transform.localPosition + finalPosition, radius, invalidLayers);
        while (destinationColliders.Length > 0) {
            distance = distance - step;
            finalPosition = blinkDirection * distance;
            destinationColliders = Physics2D.OverlapCircleAll(player.transform.localPosition + finalPosition, radius, invalidLayers);
        }

        return finalPosition;
    }

    private void StopBlinking(PlayerFSM player) {
        player.rb.gravityScale = originalGravity;
        player.rb.velocity = Vector2.zero;
        player.blinkCooldownTimer = player.config.startBlinkCooldownTime;
    }

    private void InputBuffer() {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
    }
}
