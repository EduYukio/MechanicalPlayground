using UnityEngine;

public class PlayerGunBootsState : PlayerBaseState {
    private float bulletSpeed;
    private float upwardsVelocityBoost;
    private float maxPlayerVelocity;

    public override void EnterState(PlayerFSM player) {
        Setup(player);
        PlayAnimation(player);
    }

    public override void Update(PlayerFSM player) {
        base.ProcessHorizontalMoveInput(player);

        if (Input.GetAxisRaw("Gun Boots") > 0 || Input.GetButton("Gun Boots")) {
            GunBootsAction(player);
            return;
        }

        if (CheckTransitionToFalling(player)) return;
        if (base.CheckTransitionToAttacking(player)) return;
        if (base.CheckTransitionToExploding(player)) return;
        if (base.CheckTransitionToDashing(player)) return;
        if (base.CheckTransitionToBlinking(player)) return;
        if (base.CheckTransitionToGrounded(player)) return;
        if (base.CheckTransitionToDoubleJumping(player)) return;
    }

    private void Setup(PlayerFSM player) {
        bulletSpeed = player.config.bootsBulletSpeed;
        upwardsVelocityBoost = player.config.gunBootsUpwardsBoost;
        maxPlayerVelocity = player.config.gunBootsMaxPlayerVelocity;
    }

    private void PlayAnimation(PlayerFSM player) {
        player.animator.Play("PlayerGunBoots");
    }

    private void GunBootsAction(PlayerFSM player) {
        if (player.gunBootsCooldownTimer > 0) return;

        Manager.audio.Play("Gun Shoot");
        player.gunBootsCooldownTimer = player.config.startGunBootsCooldownTime;
        ShootBullet(player);
        SetPlayerVerticalVelocity(player);
    }

    private void ShootBullet(PlayerFSM player) {
        float xOffset = UnityEngine.Random.Range(-0.2f, 0.2f);
        Vector3 spawnPosition = player.transform.position + new Vector3(xOffset, -0.55f, 0f);
        GameObject bullet = MonoBehaviour.Instantiate(player.bootsBulletPrefab, spawnPosition, Quaternion.identity);

        bullet.GetComponent<Rigidbody2D>().velocity = Vector3.down * bulletSpeed;
    }

    private void SetPlayerVerticalVelocity(PlayerFSM player) {
        float yVelocity = player.rb.velocity.y + upwardsVelocityBoost;
        float yVelocityClamped = Mathf.Clamp(yVelocity, 0f, maxPlayerVelocity);

        player.rb.velocity = new Vector2(player.rb.velocity.x, yVelocityClamped);
    }

    public override bool CheckTransitionToFalling(PlayerFSM player) {
        if (player.isGrounded) return false;

        if (player.rb.velocity.y <= 0) {
            player.TransitionToState(player.FallingState);
            return true;
        }

        return false;
    }
}
