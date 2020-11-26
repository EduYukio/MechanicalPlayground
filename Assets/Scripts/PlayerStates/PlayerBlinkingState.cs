using UnityEngine;

public class PlayerBlinkingState : PlayerBaseState {
    public override void EnterState(PlayerFSM player) {
        // player.animator.Play("PlayerBlink");
        BlinkAction(player);
    }

    public override void Update(PlayerFSM player) {
        if (base.CheckTransitionToGrounded(player)) return;
        if (base.CheckTransitionToFalling(player)) return;
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
        player.blinkCooldownTimer = player.config.startBlinkCooldownTime;
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
}
