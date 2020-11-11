using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallChecker : MonoBehaviour {
    private Player player;
    private Collider2D selfCollider;

    private void Start() {
        player = FindObjectOfType<Player>();
        selfCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Ground")) {
            player.isTouchingWall = true;

            if (selfCollider.offset.x < 0) {
                player.isTouchingLeftWall = true;
                player.isTouchingRightWall = false;
            }
            else {
                player.isTouchingLeftWall = false;
                player.isTouchingRightWall = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Ground")) {
            player.isTouchingWall = false;
            player.isTouchingLeftWall = false;
            player.isTouchingRightWall = false;
        }
    }
}
