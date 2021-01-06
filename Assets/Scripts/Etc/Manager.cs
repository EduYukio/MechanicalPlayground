using UnityEngine;

static class Manager {
    public static AudioManager audio;

    static Manager() {
        GameObject preloadObj = GameObject.Find("PreloadObject");

        audio = preloadObj.GetComponent<AudioManager>();
    }
}