using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {
    public Slider bgmSlider;
    public Slider sfxSlider;
    private float bgmValue;
    private float sfxValue;

    private void OnEnable() {
        Manager.audio.SetBGMVolumeToNormal();

        Manager.audio.mixer.GetFloat("bgmVolume", out bgmValue);
        Manager.audio.mixer.GetFloat("sfxVolume", out sfxValue);

        bgmSlider.value = bgmValue;
        sfxSlider.value = sfxValue;
    }

    public void bgmSetVolume() {
        Manager.audio.mixer.SetFloat("bgmVolume", bgmSlider.value);
        Manager.audio.normalBGMVolume = bgmSlider.value;
    }

    public void sfxSetVolume() {
        Manager.audio.mixer.SetFloat("sfxVolume", sfxSlider.value);
    }
}
