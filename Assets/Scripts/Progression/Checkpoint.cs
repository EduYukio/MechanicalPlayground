using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {
    private Animator animator;
    public static Checkpoint currentCheckpoint = null;
    public GameObject checkpointParticles;

    private void Awake() {
        animator = GetComponent<Animator>();

        if (Checkpoint.currentCheckpoint == this) {
            Checkpoint.currentCheckpoint.GetComponent<Animator>().Play("FlagOn");
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (Checkpoint.currentCheckpoint == this) return;

        if (other.gameObject.CompareTag("Player")) {
            DeactivateCurrentCheckpoint();
            PlayerFSM.respawnPosition = transform.position;
            animator.Play("FlagActivating");
            Manager.audio.Play("Checkpoint Boing");

            float soundDelay = Helper.GetAnimationDuration("FlagActivating", animator) * 0.75f;
            Manager.audio.PlayDelayed("Checkpoint Activated", soundDelay);
            StartCoroutine(nameof(WaitForPlayParticles), soundDelay);
            Checkpoint.currentCheckpoint = this;
        }
    }

    IEnumerator WaitForPlayParticles(float soundDelay) {
        yield return new WaitForSecondsRealtime(soundDelay);
        Vector3 particlePosition = transform.position + new Vector3(-0.25f, 0.45f, 0);
        Instantiate(checkpointParticles, particlePosition, Quaternion.identity);
    }

    public static void DeactivateCurrentCheckpoint() {
        if (Checkpoint.currentCheckpoint != null) {
            Checkpoint.currentCheckpoint.GetComponent<Animator>().Play("FlagOff");
        }
    }

    public static void ResetCheckPointState() {
        DeactivateCurrentCheckpoint();
        Checkpoint.currentCheckpoint = null;
        PlayerFSM.respawnPosition = Vector3.zero;
    }
}
