using UnityEngine;

public class ClearMechanicsItem : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            ClearMechanics(other.gameObject);
            Destroy(transform.gameObject);
        }
    }

    public void ClearMechanics(GameObject otherObj) {
        PlayerFSM player = otherObj.GetComponent<PlayerFSM>();
        player.mechanics.ResetMechanics();
        player.mechanics.EnableBasicMechanics();
    }
}