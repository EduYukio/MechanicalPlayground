using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class Helper {
    public static float GetAnimationDuration(string clipName, Animator animator) {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips) {
            if (clip.name == clipName) {
                return clip.length;
            }
        }

        Debug.Log("ERROR! No clip with this name was found: " + clipName);
        return 0;
    }

    public static bool IsPlayingAnimation(string stateName, Animator animator) {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }

    public static void CheckIfNeedToBlinkRed(ref bool blinkRed, Image image) {
        if (blinkRed) {
            float step = 0.02f;

            Color currentColor = image.color;
            Color newColor = new Color(1, currentColor.g + step, currentColor.b + step);
            image.color = newColor;

            if (newColor.g >= 1f && newColor.b >= 1f) {
                blinkRed = false;
            }
        }
    }

    public static void CheckIfNeedToBlinkRed(ref bool blinkRed, TMP_Text text) {
        if (blinkRed) {
            text.enabled = false;
            text.enabled = true;
            float step = 0.02f;

            Color currentColor = text.color;
            Color newColor = new Color(currentColor.r - step, 0, 0);
            text.color = newColor;

            if (newColor.r <= 0f) {
                blinkRed = false;
            }
        }
    }
}