using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OkButton : MonoBehaviour {
    public Slots slots;
    public GameObject playerObj;
    public GameObject playerPrefab;
    public GameObject menu;

    void Start() {
        slots = FindObjectOfType<Slots>();
        playerObj = GameObject.Find("Player");
        menu = GameObject.Find("MechanicsMenu");
    }

    public void OkAction() {
        if (slots.lastIndexAvailable < 3) {
            return;
        }

        Destroy(playerObj);
        playerObj = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        foreach (string mechanic in slots.mechanics) {
            System.Type mType = System.Type.GetType(mechanic);
            playerObj.AddComponent(mType);
        }

        menu.SetActive(true);

        Player playerScript = playerObj.GetComponent<Player>();
        playerScript.menu = GameObject.Find("MechanicsMenu");

        menu.SetActive(false);
        Time.timeScale = 1;
    }
}
