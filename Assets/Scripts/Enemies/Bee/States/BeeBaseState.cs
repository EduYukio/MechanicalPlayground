using UnityEngine;

public abstract class BeeBaseState {
    public abstract void EnterState(BeeFSM bee);
    public abstract void Update(BeeFSM bee);
}