using UnityEngine;

public class Congratulations : MonoBehaviour {
    private void Start() {
        PreloadScript preload = GameObject.Find("PreloadObject").GetComponent<PreloadScript>();
        preload.currentBGM.source?.Stop();
        preload.currentBGM = new Sound();

        Manager.audio.Play("Victory Fanfare");
    }
}