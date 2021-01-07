using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {
    private Animator animator;
    public static Checkpoint currentCheckpoint = null;

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
            Manager.audio.PlayDelayed("Checkpoint Activated", Helper.GetAnimationDuration("FlagActivating", animator) * 0.5f);
            Checkpoint.currentCheckpoint = this;
        }
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
