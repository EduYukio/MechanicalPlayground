using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallChecker : MonoBehaviour {
    private Player playerScript;

    private void Start() {
        playerScript = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Ground")) {
            playerScript.isTouchingWall = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Ground")) {
            playerScript.isTouchingWall = false;
        }
    }
}
