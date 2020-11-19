using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Mechanics : ScriptableObject {
    [Header("Mechanics enabled")]
    public bool walk = false;
    public bool jump = false;
    public bool attack = false;
    public bool doubleJump = false;
    public bool dash = false;
    public bool wallSlide = false;
    public bool wallJump = false;
    public bool blink = false;

    // public bool[] GetMechanics() {
    //     return new bool[] {
    //         walk,
    //         jump,
    //         attack,
    //         doubleJump,
    //         dash,
    //         wallSlide,
    //         wallJump,
    //         blink,
    //     };
    // }

    public void ResetMechanics() {
        walk = false;
        jump = false;
        attack = false;
        doubleJump = false;
        dash = false;
        wallSlide = false;
        wallJump = false;
        blink = false;
        // bool[] mechanicsArray = GetMechanics();
        // for (int i = 0; i < mechanicsArray.Length; i++) {
        //     mechanicsArray[i] = false;
        // }
    }

    public void EnableBasicMechanics() {
        walk = true;
        jump = true;
        attack = true;
    }
}

