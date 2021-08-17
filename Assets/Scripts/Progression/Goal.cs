using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour {
    private Animator animator;
    private PlayerFSM player;
    public UnityEvent WhenPicked;

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

    private IEnumerator ChangeSceneAfterAnimation() {
        player.animator.Play("PlayerIdle");
        player.freezePlayerState = true;
        player.spriteRenderer.color = Color.white;
        Manager.audio.Play("Goal");

        yield return null;

        WhenPicked.Invoke();
        Time.timeScale = 0;
        float duration = Helper.GetAnimationDuration("GoalReached", animator);

        yield return new WaitForSecondsRealtime(duration * 2.5f);

        Time.timeScale = 1;
        Checkpoint.ResetCheckPointState();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void DisableMechanic(string name) {
        player.mechanics.Deactivate(name);
    }
}
