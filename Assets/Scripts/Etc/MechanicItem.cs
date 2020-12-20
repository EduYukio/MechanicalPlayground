using UnityEngine;

public class MechanicItem : MonoBehaviour {
    public string mechanicName;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            PlayerFSM player = other.gameObject.GetComponent<PlayerFSM>();
            player.mechanics.Activate(mechanicName);
            Destroy(gameObject);
        }
    }
}