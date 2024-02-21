using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioMixer sfxMixer;
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioMixer musicMixer;
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
        sm.musicAudioSource.clip = sm.currentSoundtrackInfo.audioClip;
        sm.musicAudioSource.volume = sm.currentSoundtrackInfo.volume;
        sm.musicAudioSource.Play();
    }

    public static void SetSoundEffectVolume(float volume)
    {
        sm.sfxMixer.SetFloat("Volume", SliderToDb(volume));
    }
    
    public static void SetMusicVolume(float volume)
    {
        sm.musicMixer.SetFloat("Volume", SliderToDb(volume));
    }

    private static float SliderToDb(float sliderVolume)
    {
        return Mathf.Log10(Mathf.Clamp(sliderVolume, Mathf.Epsilon, 1f)) * 20f + 10f;
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
            musicAudioSource.volume = currentSoundtrackInfo.volume * (1 - timePassedRelative);
            if (timePassedRelative > 1f)
            {
                musicAudioSource.Stop();
                fadingOut = false;
            }
        }
    }
}
