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

        Debug.LogError("ERROR! No clip with this name was found: " + clipName);
        return 0;
    }

    public static void PlayAnimationIfPossible(string animationToPlay, Animator animator, string[] waitAnimations) {
        foreach (var animation in waitAnimations) {
            if (IsPlayingAnimation(animation, animator)) return;
        }

        animator.Play(animationToPlay);
    }

    public static bool IsPlayingAnimation(string stateName, Animator animator) {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }

    public static void CheckIfNeedToBlinkRed(ref bool mustBlinkRed, Image image) {
        if (mustBlinkRed) {
            float step = 0.02f;

            Color currentColor = image.color;
            Color newColor = new Color(1, currentColor.g + step, currentColor.b + step);
            image.color = newColor;

            if (newColor.g >= 1f && newColor.b >= 1f) {
                mustBlinkRed = false;
            }
        }
    }

    public static void CheckIfNeedToBlinkRed(ref bool mustBlinkRed, TMP_Text text) {
        if (mustBlinkRed) {
            // Black magic that fix a text disappearing bug
            text.enabled = false;
            text.enabled = true;

            float step = 0.02f;

            Color currentColor = text.color;
            Color newColor = new Color(currentColor.r - step, 0, 0);
            text.color = newColor;

            if (newColor.r <= 0f) {
                mustBlinkRed = false;
            }
        }
    }

    public static void InputBuffer(out float xInput, out float yInput) {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
    }
}