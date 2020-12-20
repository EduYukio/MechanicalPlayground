using UnityEngine;

public class ClearMechanicsItem : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            PlayerFSM player = other.gameObject.GetComponent<PlayerFSM>();
            player.mechanics.ResetMechanics();
            player.mechanics.EnableBasicMechanics();
            Destroy(transform.gameObject);
        }
    }
}