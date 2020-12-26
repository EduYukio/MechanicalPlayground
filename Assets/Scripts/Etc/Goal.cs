using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour {
    private Animator animator;
    private PlayerFSM player;

    private void Start() {
        animator = GetComponent<Animator>();
        player = GameObject.FindObjectOfType<PlayerFSM>();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            animator.Play("GoalReached");
            StartCoroutine(ChangeSceneAfterAnimation());
        }
    }

    IEnumerator ChangeSceneAfterAnimation() {
        player.animator.Play("PlayerIdle");
        player.freezePlayerState = true;
        player.spriteRenderer.color = Color.white;
        yield return null;
        Time.timeScale = 0;

        float duration = Helper.GetAnimationDuration("GoalReached", animator);
        yield return new WaitForSecondsRealtime(duration * 2.5f);
        Time.timeScale = 1;
        Checkpoint.ResetCheckPointState();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
