using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour {
    private Player playerScript;

    private void Start() {
        playerScript = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        string otherTag = other.gameObject.tag;
        if (otherTag == "Ground") {
            playerScript.isGrounded = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        string otherTag = other.gameObject.tag;
        if (otherTag == "Ground") {
            playerScript.isGrounded = false;
        }
    }
}
