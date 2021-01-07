using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            PlayerFSM player = other.gameObject.GetComponent<PlayerFSM>();
            if (player.keys.Count > 0) {
                Manager.audio.Play("Open Gate");
                ConsumeKey(player);
                Destroy(gameObject, 0.1f);
            }
        }
    }

    void ConsumeKey(PlayerFSM player) {
        GameObject key = player.keys[player.keys.Count - 1];
        player.keys.RemoveAt(player.keys.Count - 1);
        key.GetComponentInChildren<Key>().RestoreOneSlot();
        Destroy(key, 0.1f);
    }
}
