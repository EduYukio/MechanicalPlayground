using UnityEngine;

public class Congratulations : MonoBehaviour {
    private void Start() {
        PreloadScript preload = GameObject.Find("PreloadObject").GetComponent<PreloadScript>();
        if (preload.currentBGM.source != null && preload.currentBGM.source.isPlaying) {
            preload.currentBGM.source.Stop();
        }
        preload.currentBGM = new Sound();

        Manager.audio.Play("Victory Fanfare");
    }
}