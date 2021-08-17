using System.Collections;
using UnityEngine;

public class Shield : MonoBehaviour {
    public PlayerFSM player;
    private bool canDefend = true;

    private void Update() {
        gameObject.transform.position = player.transform.position + new Vector3(0, -0.1f, 0);
    }

    private void OnEnable() {
        gameObject.transform.position = player.transform.position + new Vector3(0, -0.1f, 0);
    }

    public void CheckShieldInput() {
        if (!player.mechanics.IsEnabled("Shield")) return;

        CheckForParryInput();

        if (Input.GetButtonUp("Shield")) {
            canDefend = true;
        }

        if (!player.isParrying) {
            if (Input.GetButton("Shield") && canDefend && player.shieldCooldownTimer <= 0) {
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

    public void Parry(GameObject parriedObject) {
        if (!player.mechanics.IsEnabled("Parry")) return;
        Manager.audio.Play("Parry");
        StartCoroutine(nameof(ParryCoroutine), parriedObject);
    }

    public IEnumerator ParryCoroutine(GameObject parriedObject) {
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

        if (player.mechanics.IsEnabled("Reflect Projectile")) {
            ReflectBullet(parriedObject);
        }
        else {
            Destroy(parriedObject);
        }
    }

    public void ConsumeShield() {
        Manager.audio.Play("Shield Consumed");
        gameObject.SetActive(false);
        canDefend = false;
        player.shieldCooldownTimer = player.config.startShieldCooldownTime;
        Manager.audio.FindSound("Shield Returning").source.PlayDelayed(player.shieldCooldownTimer * 0.85f);
    }

    public void ReflectBullet(GameObject parriedObject) {
        EnemyBullet bullet = parriedObject.GetComponent<EnemyBullet>();
        if (bullet == null) return;

        bullet.alreadyProcessedHit = false;
        bullet.rb.velocity = -4 * bullet.rb.velocity;
        Vector3 angle = bullet.transform.eulerAngles;
        bullet.transform.eulerAngles = new Vector3(angle.x, angle.y, angle.z + 180);
        if (bullet.name.Contains("Ethereal")) {
            bullet.gameObject.layer = LayerMask.NameToLayer("EtherealParriedBullet");
        }
        else {
            bullet.gameObject.layer = LayerMask.NameToLayer("ParriedBullet");
        }
    }
}