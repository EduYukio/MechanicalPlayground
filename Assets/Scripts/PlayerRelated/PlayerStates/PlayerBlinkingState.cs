using UnityEngine;

public class PlayerBlinkingState : PlayerBaseState
{
    private float beginBlinkTimer, endBlinkTimer;
    private float xInput, yInput;
    private float originalGravity;
    private bool alreadyBlinked;

    public override void EnterState(PlayerFSM player)
    {
        Setup(player);
        PlayAnimation(player);
        PlayAudio();
    }

    public override void FixedUpdate(PlayerFSM player)
    {
        if (beginBlinkTimer > 0)
        {
            beginBlinkTimer -= Time.deltaTime;
            return;
        }

        if (!alreadyBlinked)
        {
            player.animator.Play("PlayerAppear");
            BlinkAction(player);
            alreadyBlinked = true;
        }

        if (endBlinkTimer > 0)
        {
            endBlinkTimer -= Time.deltaTime;
            return;
        }

        StopBlinking(player);

        if (base.CheckTransitionToGunBoots(player)) return;
        if (base.CheckTransitionToGrounded(player)) return;
        if (base.CheckTransitionToFalling(player)) return;
    }

    private void Setup(PlayerFSM player)
    {
        beginBlinkTimer = player.config.startPreBlinkTime;
        endBlinkTimer = player.config.startPostBlinkTime;
        originalGravity = player.rb.gravityScale;
        alreadyBlinked = false;
        player.rb.velocity = Vector2.zero;
        player.rb.gravityScale = 0f;
        Helper.InputBuffer(out xInput, out yInput);
    }

    private void PlayAnimation(PlayerFSM player)
    {
        player.animator.Play("PlayerDisappear");
    }

    private void PlayAudio()
    {
        Manager.audio.Play("Blink");
    }

    private void BlinkAction(PlayerFSM player)
    {
        Vector3 blinkDirection = base.GetFourDirectionalInput(player, xInput, yInput);
        Vector3 destinationPosition = ValidDestinationPosition(player, blinkDirection);
        player.transform.Translate(destinationPosition);
    }

    private Vector3 ValidDestinationPosition(PlayerFSM player, Vector3 blinkDirection)
    {
        float distance = player.config.blinkDistance;
        Vector3 finalPosition = blinkDirection * distance;
        LayerMask invalidLayers = LayerMask.GetMask("Ground", "Gate");
        float radius = player.config.blinkGroundCheckRadius;
        float step = 0.1f;

        Collider2D[] invalidColliders = Physics2D.OverlapCircleAll(player.transform.localPosition + finalPosition, radius, invalidLayers);
        while (invalidColliders.Length > 0)
        {
            distance = distance - step;
            finalPosition = blinkDirection * distance;
            invalidColliders = Physics2D.OverlapCircleAll(player.transform.localPosition + finalPosition, radius, invalidLayers);
        }

        return finalPosition;
    }

    private void StopBlinking(PlayerFSM player)
    {
        player.rb.gravityScale = originalGravity;
        player.rb.velocity = Vector2.zero;
        player.blinkCooldownTimer = player.config.startBlinkCooldownTime;
    }
}
