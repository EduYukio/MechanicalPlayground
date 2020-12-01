using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {
    public static bool activated = false;
    private Animator animator;

    private void Start() {
        animator = GetComponent<Animator>();
        animator.SetBool("activated", activated);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (activated) return;

        if (other.gameObject.CompareTag("Player")) {
            PlayerFSM.respawnPosition = transform.position;
            animator.Play("FlagActivating");
            activated = true;
        }
    }
}
