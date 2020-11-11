using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallChecker : MonoBehaviour {
    private Player player;

    private void Start() {
        player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Ground")) {
            player.isTouchingWall = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Ground")) {
            player.isTouchingWall = false;
        }
    }
}
