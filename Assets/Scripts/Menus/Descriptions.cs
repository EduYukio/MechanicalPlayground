using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Descriptions : ScriptableObject {
    public Dictionary<string, string> dict;

    [Header("Movement")]
    public string doubleJump = "Jump again while in the air.";
    public string wallSlide = "Slide on walls.";
    public string wallJump = "Jump while sliding on the walls.";
    public string dash = "Rapid burst of forward movement.";
    public string etherealDash = "You pass through enemies and obstacles while dashing (not walls and gates).";
    public string blink = "Medium range teleport that can pass through anything (including walls and gates).";

    [Header("Attack")]
    public string attack = "Melee attack that can damage enemies.";
    public string rangeBoost = "More range on your attacks.";
    public string destroyProjectile = "Your attacks destroy projectiles.";
    public string pogoJump = "Attack enemies and obstacles downwards while jumping to do another jump.";
    public string gunBoots = "Shoot bullets from your boots, damaging enemies and launching yourself upwards.";
    public string explosion = "Create a low range explosion that can destroy enemies, obstacles and gates.";

    [Header("Utility")]
    public string shield = "Block enemy projectiles. After it blocks, it is consumed and takes a second to be used again.";
    public string parry = "If you block right at the moment of impact, the shield will not be consumed and you can reuse it immediately.";
    public string reflectProjectile = "Parried projectiles will be reflected to the enemy.";
    public string spikeInvul = "You will no longer take damage from spikes.";
    public string sawInvul = "You will no longer take damage from saws.";
    public string createPlatform = "Create platforms underneath you. You can jump from them and they also block enemy projectiles.";


    public Dictionary<string, string> GetDictionary() {
        dict = new Dictionary<string, string>() {
            {"Double Jump", doubleJump},
            {"Wall Slide", wallSlide},
            {"Wall Jump", wallJump},
            {"Dash", dash},
            {"Ethereal Dash", etherealDash},
            {"Blink", blink},
            {"Attack", attack},
            {"Range Boost", rangeBoost},
            {"Destroy Projectile", destroyProjectile},
            {"Pogo Jump", pogoJump},
            {"Gun Boots", gunBoots},
            {"Explosion", explosion},
            {"Shield", shield},
            {"Parry", parry},
            {"Reflect Projectile", reflectProjectile},
            {"Saw Invulnerability", sawInvul},
            {"Spike Invulnerability", spikeInvul},
            {"Create Platform", createPlatform}
        };

        return dict;
    }

    public string GetDescription(string mechanicName) {
        dict = GetDictionary();
        if (!dict.ContainsKey(mechanicName)) return "";

        return dict[mechanicName];
    }
}

