using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public AudioMixer mixer;
    public Sound[] sounds;

    public float normalBGMVolume { get; set; }
    public float muffleEffect { get; set; }

    void Awake() {
        normalBGMVolume = 0f;
        muffleEffect = 12.5f;

        foreach (Sound sound in sounds) {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.loop = sound.loop;

            sound.source.outputAudioMixerGroup = sound.mixerGroup;
        }
    }

    public void Play(string soundName) {
        Sound sound = FindSound(soundName);
        if (sound == null) return;

        SetSourceSettings(sound);

        sound.source.Play();
    }

    public void PlayDelayed(string soundName, float delay) {
        Sound sound = FindSound(soundName);
        if (sound == null) return;

        SetSourceSettings(sound);

        sound.source.PlayDelayed(delay);
    }

    private void SetSourceSettings(Sound sound) {
        sound.source.volume = sound.volume * (1f + UnityEngine.Random.Range(-sound.volumeVariance / 2f, sound.volumeVariance / 2f));
        sound.source.pitch = sound.pitch * (1f + UnityEngine.Random.Range(-sound.pitchVariance / 2f, sound.pitchVariance / 2f));
    }

    public Sound FindSound(string soundName) {
        Sound sound = Array.Find(sounds, (Predicate<Sound>)(item => item.name == soundName));
        if (sound == null) {
            Debug.LogWarning("Sound: " + soundName + " not found!");
            return null;
        }

        return sound;
    }

    public void SetBGMVolumeToNormal() {
        mixer.SetFloat("bgmVolume", normalBGMVolume);
    }

    public void SetBGMVolumeToMuffled() {
        mixer.SetFloat("bgmVolume", normalBGMVolume - muffleEffect);
    }
}
