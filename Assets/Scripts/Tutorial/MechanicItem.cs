using UnityEngine;
using UnityEngine.Events;

public class MechanicItem : MonoBehaviour {
    public string mechanicName;
    public UnityEvent WhenPicked;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            Manager.audio.Play("Mechanic Item");
            WhenPicked.Invoke();
            PlayerFSM player = other.gameObject.GetComponent<PlayerFSM>();
            player.mechanics.Activate(mechanicName);
            Destroy(transform.parent.gameObject);
        }
    }

    public void DisableMechanic(string name) {
        PlayerFSM player = GameObject.FindObjectOfType<PlayerFSM>();
        player.mechanics.Deactivate(name);
    }
}