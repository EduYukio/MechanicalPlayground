using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDyingState : PlayerBaseState
{
    private float dyingTimer;

    public override void EnterState(PlayerFSM player)
    {
        Setup(player);
        PlayAnimation(player);
        PlayAudio();
        PlayParticles(player);
        ShakeCamera(player);
        DieAction(player);
    }

    public override void FixedUpdate(PlayerFSM player)
    {
        if (dyingTimer > 0)
        {
            dyingTimer -= Time.deltaTime;
            return;
        }

        RealodScene();
    }

    private void Setup(PlayerFSM player)
    {
        dyingTimer = 0.5f;
        player.isDying = true;
        player.spriteRenderer.color = Color.white;
    }

    private void PlayAnimation(PlayerFSM player)
    {
        player.animator.Play("PlayerHit");
    }

    private void PlayAudio()
    {
        Manager.audio.Play("PlayerDying");
    }

    private void PlayParticles(PlayerFSM player)
    {
        player.dyingParticles.Play();
    }

    private void ShakeCamera(PlayerFSM player)
    {
        float shakeDuration = player.config.dyingShakeDuration;
        float shakeMagnitude = player.config.dyingShakeMagnitude;

        Manager.shaker.Shake(player.cameraObj, shakeDuration, shakeMagnitude);
        player.cameraHolder.transform.parent = null;
    }

    private void DieAction(PlayerFSM player)
    {
        float direction = -player.lookingDirection;
        player.rb.velocity = new Vector3(direction * 3f, 6f, 0);
        player.rb.constraints = RigidbodyConstraints2D.None;
        player.rb.angularVelocity = direction * -40f;
        player.rb.gravityScale = 2f;
        player.GetComponent<Collider2D>().enabled = false;
    }

    private void RealodScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
