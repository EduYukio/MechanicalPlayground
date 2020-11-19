using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Mechanics : ScriptableObject {
    private List<bool> mechanicsList;

    [Header("Mechanics enabled")]
    public bool walk = false;
    public bool jump = false;
    public bool attack = false;
    public bool doubleJump = false;
    public bool dash = false;
    public bool wallSlide = false;
    public bool wallJump = false;
    public bool blink = false;

    private void Awake() {
        mechanicsList = new List<bool>();
        mechanicsList.Add(walk);
        mechanicsList.Add(jump);
        mechanicsList.Add(doubleJump);
        mechanicsList.Add(dash);
        mechanicsList.Add(attack);
        mechanicsList.Add(wallSlide);
        mechanicsList.Add(wallJump);
        mechanicsList.Add(blink);
    }

    public void ResetMechanics() {
        for (int i = 0; i < mechanicsList.Count; i++) {
            mechanicsList[i] = false;
        }
    }

    public void EnableBasicMechanics() {
        walk = true;
        jump = true;
        attack = true;
    }
}

