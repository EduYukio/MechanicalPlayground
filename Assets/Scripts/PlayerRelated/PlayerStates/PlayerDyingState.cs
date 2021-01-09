using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDyingState : PlayerBaseState {
    public float dyingTimer;

    public override void EnterState(PlayerFSM player) {
        Setup(player);
    }

    public override void Update(PlayerFSM player) {
        if (dyingTimer > 0) {
            dyingTimer -= Time.deltaTime;
            return;
        }

        DieAction(player);
    }

    void Setup(PlayerFSM player) {
        player.dyingParticles.Play();
        player.isDying = true;
        player.spriteRenderer.color = Color.white;

        player.animator.SetFloat("disappearSpeedMultiplier", 1f);
        dyingTimer = 0.5f;
        player.animator.Play("PlayerHit");
        Manager.audio.Play("PlayerDying");

        //TODO: refatorar isso aqui
        Manager.shaker.Shake(player.cameraObj, player.config.dyingShakeDuration, player.config.dyingShakeMagnitude);
        player.cameraHolder.transform.parent = null;

        float direction = -player.lastDirection;
        player.rb.bodyType = RigidbodyType2D.Dynamic;
        player.rb.velocity = new Vector3(direction * 3f, 6f, 0);
        player.rb.constraints = RigidbodyConstraints2D.None;
        player.rb.angularVelocity = direction * -40f;
        player.rb.gravityScale = 2f;
        player.GetComponent<Collider2D>().enabled = false;
    }

    void DieAction(PlayerFSM player) {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
