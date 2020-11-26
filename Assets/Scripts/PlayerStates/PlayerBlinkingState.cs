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

        Vector3 newPos = player.config.blinkDistance * blinkDirection;
        player.transform.Translate(newPos);
        player.blinkCooldownTimer = player.config.startBlinkCooldownTime;
    }
}
