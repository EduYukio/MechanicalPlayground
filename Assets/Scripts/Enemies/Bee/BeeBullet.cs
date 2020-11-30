using UnityEngine;

public class BeeBullet : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            PlayerFSM player = other.gameObject.GetComponent<PlayerFSM>();
            player.TransitionToState(player.DyingState);
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("Ground")) {
            Destroy(gameObject);
        }
    }
}