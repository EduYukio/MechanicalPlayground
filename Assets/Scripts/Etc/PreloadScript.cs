using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreloadScript : MonoBehaviour {
    void Awake() {
        DontDestroyOnLoad(gameObject);
        Manager.audio.Play("BGM1");

        // #if UNITY_EDITOR
        if (PreloadInitializer.selectedScene > 0) {
            SceneManager.LoadScene(PreloadInitializer.selectedScene);
        }
        else {
            SceneManager.LoadScene(1);
        }
        // #endif
    }
}
