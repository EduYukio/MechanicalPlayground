using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : MonoBehaviour {
    public Jump jp;

    void Start() {
        jp = GetComponent<Jump>();
    }

    void Update() {
        if (Input.GetButtonDown("Jump") && jp != null && jp.jumpQuantity == 1) {
            jp.JumpAction(Vector2.up, false);
        }
    }
}
