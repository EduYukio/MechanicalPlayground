using System.Collections;
using UnityEngine;

public class Checkpoint : MonoBehaviour {
    private Animator animator;
    public static string activatedName = "";
    public GameObject checkpointParticles;

    private void Awake() {
        animator = GetComponent<Animator>();

        if (Checkpoint.activatedName == gameObject.name) {
            animator.Play("FlagOn");
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.gameObject.CompareTag("Player")) return;
        if (Checkpoint.activatedName == gameObject.name) return;

        DeactivateCurrentCheckpoint();
        PlayerFSM.respawnPosition = transform.position;
        animator.Play("FlagActivating");
        Manager.audio.Play("Checkpoint Boing");

        float soundDelay = Helper.GetAnimationDuration("FlagActivating", animator) * 0.75f;
        Manager.audio.PlayDelayed("Checkpoint Activated", soundDelay);
        StartCoroutine(nameof(WaitForPlayParticles), soundDelay);
        Checkpoint.activatedName = gameObject.name;
    }

    private IEnumerator WaitForPlayParticles(float soundDelay) {
        yield return new WaitForSecondsRealtime(soundDelay);
        Vector3 particlePosition = transform.position + new Vector3(-0.25f, 0.45f, 0);
        Instantiate(checkpointParticles, particlePosition, Quaternion.identity);
    }

    public static void DeactivateCurrentCheckpoint() {
        if (Checkpoint.activatedName != "") {
            Checkpoint activatedCheckpoint = GameObject.Find(activatedName).GetComponent<Checkpoint>();
            activatedCheckpoint?.animator.Play("FlagOff");
            Checkpoint.activatedName = "";
        }
    }

    public static void ResetCheckPointState() {
        DeactivateCurrentCheckpoint();
        PlayerFSM.respawnPosition = Vector3.zero;
    }
}
