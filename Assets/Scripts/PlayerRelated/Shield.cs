using System.Collections;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private PlayerFSM player = null;
    private bool canDefend = true;

    public void CheckShieldInput()
    {
        if (!player.mechanics.IsEnabled("Shield")) return;
        if (JustDeactivatedShield()) return;

        CheckForParryInput();

        if (!player.isParrying)
        {
            if (Input.GetButton("Shield") && canDefend && player.shieldCooldownTimer <= 0)
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void CheckForParryInput()
    {
        if (!player.mechanics.IsEnabled("Parry")) return;

        if (Input.GetButtonDown("Shield"))
        {
            player.parryTimer = player.config.startParryTime;
        }
    }

    private bool JustDeactivatedShield()
    {
        if (Input.GetButtonUp("Shield"))
        {
            canDefend = true;
            return true;
        }

        return false;
    }

    public void Parry(GameObject parriedObject)
    {
        if (!player.mechanics.IsEnabled("Parry")) return;

        Manager.audio.Play("Parry");
        StartCoroutine(nameof(ParryEffectCoroutine), parriedObject);
    }

    public IEnumerator ParryEffectCoroutine(GameObject parriedObject)
    {
        player.isParrying = true;
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(player.config.parryPauseDuration);

        Time.timeScale = 1f;
        player.isParrying = false;
        gameObject.SetActive(false);

        if (!Input.GetButton("Shield"))
        {
            canDefend = true;
        }
        else
        {
            canDefend = false;
        }

        if (player.mechanics.IsEnabled("Reflect Projectile"))
        {
            EnemyBullet bulletScript = parriedObject.GetComponent<EnemyBullet>();
            bulletScript.ReflectBullet();
        }
        else
        {
            Destroy(parriedObject);
        }
    }

    public void ConsumeShield()
    {
        Manager.audio.Play("Shield Consumed");
        gameObject.SetActive(false);
        canDefend = false;
        player.shieldCooldownTimer = player.config.startShieldCooldownTime;
        Manager.audio.FindSound("Shield Returning").source.PlayDelayed(player.shieldCooldownTimer * 0.85f);
    }
}