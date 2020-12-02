using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            PlayerFSM player = other.gameObject.GetComponent<PlayerFSM>();
            if (player.items > 0) {
                // animação, som do portão sumindo
                player.items--;
                Destroy(gameObject);
            }
        }
    }
}
