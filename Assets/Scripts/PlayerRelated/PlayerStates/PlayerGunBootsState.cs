using UnityEngine;

public class PlayerGunBootsState : PlayerBaseState {
    public override void EnterState(PlayerFSM player) {
        Setup(player);
    }

    public override void Update(PlayerFSM player) {
        base.ProcessMovementInput(player);

        while (Input.GetButton("Gun Boots")) {
            GunBootsAction(player);
            return;
        }

        // PlayAnimationIfCan(player);

        if (CheckTransitionToFalling(player)) return;
        if (base.CheckTransitionToWalking(player)) return;
        if (base.CheckTransitionToAttacking(player)) return;
        if (base.CheckTransitionToShotgunning(player)) return;
        if (base.CheckTransitionToDashing(player)) return;
        if (base.CheckTransitionToBlinking(player)) return;
        if (base.CheckTransitionToGrounded(player)) return;
        if (base.CheckTransitionToDoubleJumping(player)) return;
    }

    void Setup(PlayerFSM player) {
    }

    void GunBootsAction(PlayerFSM player) {
        if (player.gunBootsCooldownTimer > 0) return;

        float xOffset = UnityEngine.Random.Range(-0.2f, 0.2f);
        Vector3 spawnPosition = player.transform.position + new Vector3(xOffset, -0.55f, 0f);
        GameObject bullet = MonoBehaviour.Instantiate(player.bootsBulletPrefab, spawnPosition, Quaternion.identity);
        Vector3 direction = Vector3.down;

        bullet.GetComponent<Rigidbody2D>().velocity = direction * player.config.bootsBulletSpeed;

        player.gunBootsCooldownTimer = player.config.startGunBootsCooldownTimer;

        float ySpeed = Mathf.Clamp(player.rb.velocity.y + player.config.gunBootsForce, 0f, player.config.gunBootsMaxSpeed);
        player.rb.velocity = new Vector2(player.rb.velocity.x, ySpeed);
    }

    public override bool CheckTransitionToFalling(PlayerFSM player) {
        if (player.isGrounded) return false;

        bool playerIsFalling = player.rb.velocity.y <= 0;

        if (playerIsFalling) {
            player.TransitionToState(player.FallingState);
            return true;
        }

        return false;
    }

    // private void PlayAnimationIfCan(PlayerFSM player) {
    //     if (IsPlayingAnimation("PlayerJump", player)) return;
    //     if (IsPlayingAnimation("PlayerAttacking", player)) return;
    //     if (IsPlayingAnimation("PlayerAttackingBoosted", player)) return;

    //     player.animator.Play("PlayerJump");
    // }
}