using UnityEngine;

public abstract class PlayerBaseState {
    public abstract void EnterState(PlayerFSM player);

    public abstract void Update(PlayerFSM player);

    public void ProcessMovementInput(PlayerFSM player) {
        float xInput = Input.GetAxisRaw("Horizontal");
        int direction = 0;
        if (xInput > 0) {
            direction = 1;
            player.lastDirection = direction;
        }
        else if (xInput < 0) {
            direction = -1;
            player.lastDirection = direction;
        }

        player.rb.velocity = new Vector2(direction * player.config.moveSpeed, player.rb.velocity.y);
    }
}
