using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {
    public static List<Vector3> slots;
    public Vector3 slotPosition;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            PlayerFSM player = other.gameObject.GetComponent<PlayerFSM>();
            transform.parent.SetParent(player.transform);
            PositionKey();
            transform.parent.localScale = new Vector3(1.25f, 1.25f, 1);
            player.keys.Add(transform.parent.gameObject);

            GetComponent<Collider2D>().enabled = false;
        }
    }

    void PositionKey() {
        if (Key.slots.Count > 0) {
            int randomSlotIndex = Random.Range(0, Key.slots.Count);
            Vector3 newPosition = Key.slots[randomSlotIndex];
            slotPosition = newPosition;
            transform.parent.localPosition = newPosition;
            Key.slots.Remove(newPosition);
        }
        else {
            Debug.Log("WARNING: Player got more keys than available slot positions");
            transform.parent.localPosition = new Vector3(0, 1f, 0);
        }
    }

    public static void ResetAllSlots() {
        Key.slots = new List<Vector3>();
        Key.slots.Add(new Vector3(-1, 1, 1));
        Key.slots.Add(new Vector3(-1, 0, 1));
        Key.slots.Add(new Vector3(-1, -1, 1));
        Key.slots.Add(new Vector3(1, 1, 1));
        Key.slots.Add(new Vector3(1, 0, 1));
        Key.slots.Add(new Vector3(1, -1, 1));
    }

    public void RestoreOneSlot() {
        Key.slots.Add(slotPosition);
    }
}
