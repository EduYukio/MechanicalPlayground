﻿using UnityEngine;

public class TrunkDyingState : TrunkBaseState {
    public override void EnterState(TrunkFSM trunk) {
        Enemy.DieAction(trunk.gameObject);
    }

    public override void Update(TrunkFSM trunk) {
    }
}