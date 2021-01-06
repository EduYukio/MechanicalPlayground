using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public AudioMixerGroup mixerGroup;
    public Sound[] sounds;

    void Awake() {
        foreach (Sound sound in sounds) {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.loop = sound.loop;

            sound.source.outputAudioMixerGroup = mixerGroup;
        }
    }

    public void Play(string soundName) {
        Sound sound = Array.Find(sounds, (Predicate<Sound>)(item => item.name == soundName));
        if (sound == null) {
            Debug.LogWarning("Sound: " + soundName + " not found!");
            return;
        }

        sound.source.volume = sound.volume * (1f + UnityEngine.Random.Range(-sound.volumeVariance / 2f, sound.volumeVariance / 2f));
        sound.source.pitch = sound.pitch * (1f + UnityEngine.Random.Range(-sound.pitchVariance / 2f, sound.pitchVariance / 2f));

        sound.source.Play();
    }
}
