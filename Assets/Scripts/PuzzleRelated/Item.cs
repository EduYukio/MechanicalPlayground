using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            //play collect animation
            PlayerFSM player = other.gameObject.GetComponent<PlayerFSM>();
            player.items++;
            Destroy(gameObject);
        }
    }
}
