using System.Collections;
using UnityEngine;

public class DebugManager : MonoBehaviour {
    [SerializeField] private bool canActivateSlowMotion = false;
    [SerializeField] private bool activateSlowMotion = false;
    [SerializeField] private bool printDebugStates = false;
    [SerializeField] private string debugState = "";

    [SerializeField] private Vector3 originalPosition = new Vector3(0, 0, 0);

    public void IfDebugSetRespawnPosition(PlayerFSM player) {
        if (!player.IgnoreCheckpoints) {
            if (PlayerFSM.respawnPosition == Vector3.zero) {
                PlayerFSM.respawnPosition = originalPosition;
            }
            transform.position = PlayerFSM.respawnPosition;
        }
    }

    public void IfDebugActivateSlowMotion() {
        if (canActivateSlowMotion) {
            if (activateSlowMotion) Time.timeScale = 0.2f;
            else Time.timeScale = 1f;
        }
    }

    public void IfDebugPrintStates(PlayerFSM player) {
        if (printDebugStates) {
            debugState = player.CurrentState.GetType().Name;
            Debug.Log(debugState);
        }
    }
}
