using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioSource soundtrackAudioSource;
    [SerializeField] private SoundEffectMapping soundEffectMapping;

    private static SoundManager sm;

    private Dictionary<SoundName, Sound> sounds = new();
    private AudioClipInfo currentSoundtrackInfo;
    private bool fadingOut;
    private float fadeOutDuration;
    private float fadeOutStartTime;

    public static void PlaySoundEffect(SoundName soundName)
    {
        AudioClipInfo soundInfo = sm.sounds[soundName].GetAudioClipInfo();
        sm.sfxAudioSource.PlayOneShot(soundInfo.audioClip, soundInfo.volume);
    }

    public static void SetSoundtrack(SoundName soundName)
    {
        sm.currentSoundtrackInfo = sm.sounds[soundName].GetAudioClipInfo();
        sm.soundtrackAudioSource.clip = sm.currentSoundtrackInfo.audioClip;
        sm.soundtrackAudioSource.volume = sm.currentSoundtrackInfo.volume;
        sm.soundtrackAudioSource.Play();
    }

    public static void FadeOutSoundtrack(float fadeOutDuration)
    {
        sm.fadingOut = true;
        sm.fadeOutDuration = fadeOutDuration;
        sm.fadeOutStartTime = Time.unscaledTime;
    }

    private void Awake()
    {
        if (sm != null)
        {
            Debug.LogWarning("More than 1 SoundManager in the scene");
            return;
        }
        sm = this;

        Dictionary<SoundName, List<AudioClipInfo>> tempDict = new();

        foreach (AudioClipInfo audioClipInfo in soundEffectMapping.audioClipInfoArray)
        {
            if (tempDict.ContainsKey(audioClipInfo.soundName)) {
                tempDict[audioClipInfo.soundName].Add(audioClipInfo);
            }
            else
            {
                tempDict.Add(audioClipInfo.soundName, new List<AudioClipInfo>() { audioClipInfo });
            }
        }

        foreach (var kv in tempDict)
        {
            if (kv.Value.Count == 1)
            {
                sounds[kv.Key] = new SimpleSound(kv.Value[0]);
            }
            else
            {
                // NOTE: Maybe there should be an option to allow repeating sounds
                sounds[kv.Key] = new RandomSound(kv.Value.ToArray(), false);
            }
        }
    }

    private void Update()
    {
        if (fadingOut)
        {
            float timePassedRelative = (Time.unscaledTime - fadeOutStartTime) / fadeOutDuration;
            soundtrackAudioSource.volume = currentSoundtrackInfo.volume * (1 - timePassedRelative);
            if (timePassedRelative > 1f)
            {
                soundtrackAudioSource.Stop();
                fadingOut = false;
            }
        }
    }
}
