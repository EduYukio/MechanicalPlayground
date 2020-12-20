using UnityEngine;
using UnityEngine.Events;

public class MechanicItem : MonoBehaviour {
    public string mechanicName;
    public UnityEvent WhenPicked;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            PlayerFSM player = other.gameObject.GetComponent<PlayerFSM>();
            player.mechanics.Activate(mechanicName);
            WhenPicked.Invoke();
            Destroy(transform.parent.gameObject);
        }
    }
}