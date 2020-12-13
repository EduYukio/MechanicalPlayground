using System.Collections;
using UnityEngine;

public class Shield : MonoBehaviour {
    public PlayerFSM player;
    bool canDefend = true;

    private void Update() {
        gameObject.transform.position = player.transform.position;
    }

    public void CheckShieldInput() {
        if (!player.mechanics.IsEnabled("Shield")) return;

        CheckForParryInput();

        if (Input.GetButtonUp("Shield")) {
            canDefend = true;
        }

        if (!player.isParrying) {
            if (Input.GetButton("Shield") && canDefend) {
                gameObject.SetActive(true);
            }
            else {
                gameObject.SetActive(false);
            }
        }
    }

    public void CheckForParryInput() {
        if (!player.mechanics.IsEnabled("Parry")) return;

        if (Input.GetButtonDown("Shield")) {
            player.parryTimer = player.config.startParryTime;
        }
    }

    public void Parry() {
        if (!player.mechanics.IsEnabled("Parry")) return;

        StartCoroutine(nameof(ParryCoroutine));
    }

    public IEnumerator ParryCoroutine() {
        player.isParrying = true;
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(player.config.parryPauseDuration);
        Time.timeScale = 1f;
        player.isParrying = false;
        gameObject.SetActive(false);

        if (!Input.GetButton("Shield")) {
            canDefend = true;
        }
        else {
            canDefend = false;
        }
    }
}