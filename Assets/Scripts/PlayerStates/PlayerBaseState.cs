using UnityEngine;

public abstract class PlayerBaseState {
    public abstract void EnterState(PlayerFSM player);
    public abstract void Update(PlayerFSM player);
}
