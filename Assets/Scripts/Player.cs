using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public bool isGrounded = false;
    public bool disableControls = false;

    void Update() {
        if (disableControls) return;
    }
}
