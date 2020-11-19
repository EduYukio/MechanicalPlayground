using System.Collections;
using UnityEngine;

public class PlayerFallingState : PlayerBaseState {
    private bool playerIsFalling = false;
    private bool playerReleasedJumpButton = false;

    public override void EnterState(PlayerFSM player) {
        if (!AnimatorIsPlaying("PlayerDoubleJump", player)) {
            player.animator.Play("PlayerFall");
        }
    }

    public override void Update(PlayerFSM player) {
        BetterFalling(player);
        ProcessMovementInput(player);

        CheckTransitionToGrounded(player);
        CheckTransitionToDoubleJump(player);
        CheckTransitionToDashing(player);
    }

    void CheckTransitionToGrounded(PlayerFSM player) {
        if (player.isGrounded) {
            player.TransitionToState(player.GroundedState);
        }
    }

    void CheckTransitionToDoubleJump(PlayerFSM player) {
        if (!player.mechanics.doubleJump) return;
        if (!player.canDoubleJump) return;

        if (Input.GetButtonDown("Jump")) {
            player.TransitionToState(player.DoubleJumpingState);
        }
    }

    void BetterFalling(PlayerFSM player) {
        playerIsFalling = player.rb.velocity.y < 0;
        playerReleasedJumpButton = player.rb.velocity.y > 0 && !Input.GetButton("Jump");

        if (playerIsFalling) {
            player.rb.velocity += Vector2.up * Physics2D.gravity.y * (player.config.fallMultiplier - 1) * Time.deltaTime;
        }
        else if (playerReleasedJumpButton) {
            player.rb.velocity += Vector2.up * Physics2D.gravity.y * (player.config.lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    bool AnimatorIsPlaying(string stateName, PlayerFSM player) {
        return player.animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }
}
