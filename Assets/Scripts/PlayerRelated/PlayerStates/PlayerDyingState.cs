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
        player.rb.simulated = false;
        player.spriteRenderer.color = Color.white;

        dyingTimer = Helper.GetAnimationDuration("PlayerDisappear", player.animator);
        player.animator.SetFloat("disappearSpeedMultiplier", 1f);
        player.animator.Play("PlayerDisappear");
    }

    void DieAction(PlayerFSM player) {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
