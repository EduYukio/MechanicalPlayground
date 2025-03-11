using System.Collections.Generic;
using UnityEngine;

public class CreatePlatform : MonoBehaviour
{
    public static List<GameObject> platformsList = new List<GameObject>();

    public static void CheckCreateInput(PlayerFSM player)
    {
        if (!player.mechanics.IsEnabled("Create Platform")) return;

        if (Input.GetButtonDown("Create Platform"))
        {
            Vector3 position = player.transform.position + new Vector3(0f, -0.7f, 0f);
            GameObject platform = Instantiate(player.platformPrefab, position, Quaternion.identity);

            platformsList.Add(platform);
            Manager.audio.Play("Create Platform");
        }
    }

    public static void CheckDeleteInput(PlayerFSM player)
    {
        if (!player.mechanics.IsEnabled("Create Platform")) return;

        if (Input.GetButtonDown("Delete Platforms"))
        {
            Manager.audio.Play("Destroy Platform");

            foreach (var platform in platformsList)
            {
                Destroy(platform);
            }
        }
    }
}