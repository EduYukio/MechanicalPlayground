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
        float newX = player.lastDirection * player.config.blinkDistance;
        Vector3 newPos = new Vector3(newX, 0f, 0f);
        player.transform.Translate(newPos);
        player.blinkCooldownTimer = player.config.startBlinkCooldownTime;
    }
}
