using UnityEngine;

public static class Manager {
    public static AudioManager audio { get; private set; }
    public static CameraShaker shaker { get; private set; }

    static Manager() {
        GameObject preloadObj = GameObject.Find("PreloadObject");

        audio = preloadObj.GetComponent<AudioManager>();
        shaker = preloadObj.GetComponent<CameraShaker>();
    }
}