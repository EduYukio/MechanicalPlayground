using UnityEngine;

static class Manager {
    public static AudioManager audio;
    public static CameraShaker shaker;

    static Manager() {
        GameObject preloadObj = GameObject.Find("PreloadObject");

        audio = preloadObj.GetComponent<AudioManager>();
        shaker = preloadObj.GetComponent<CameraShaker>();
    }
}