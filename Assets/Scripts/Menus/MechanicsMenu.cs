using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MechanicsMenu : MonoBehaviour {
    public TMP_Text title;
    public TMP_Text description;
    public Mechanics mechanics;
    public VideoPlayer videoPlayer;
    public RawImage rawImage;

    void Start() {
        CleanInfo();
    }

    void Update() {

    }

    void CleanInfo() {
        rawImage.enabled = false;
        title.text = "";
        description.text = "";
    }
}
