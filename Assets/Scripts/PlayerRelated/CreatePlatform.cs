using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreatePlatform : MonoBehaviour {
    public static List<GameObject> platformsList = new List<GameObject>();

    public static void CheckCreateInput(PlayerFSM player) {
        if (!player.mechanics.IsEnabled("Create Platform")) return;

        if (Input.GetButtonDown("CreatePlatform")) {
            Vector3 newPos = player.transform.position + new Vector3(0f, -0.7f, 0f);
            GameObject platformObject = Instantiate(player.platformPrefab, newPos, Quaternion.identity);
            platformsList.Add(platformObject);
        }
    }

    public static void CheckDeleteInput(PlayerFSM player) {
        if (!player.mechanics.IsEnabled("Create Platform")) return;

        if (Input.GetButtonDown("DeletePlatforms")) {
            foreach (var platform in platformsList) {
                Destroy(platform);
            }
        }
    }
}