using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour {
    private PlayerFSM player;

    private void Start() {
        player = FindObjectOfType<PlayerFSM>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Ground")) {
            player.isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Ground")) {
            player.isGrounded = false;
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Ground")) {
            player.isGrounded = true;
        }
    }
}
