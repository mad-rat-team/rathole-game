using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClipInfo[] audioClipInfoArray;

    private static SoundManager sm;

    private AudioSource audioSource;
    private Dictionary<SoundName, Sound> sounds = new();

    public static void PlaySoundEffect(SoundName soundName)
    {
        AudioClipInfo soundInfo = sm.sounds[soundName].GetAudioClipInfo();
        sm.audioSource.PlayOneShot(soundInfo.audioClip, soundInfo.volume);
    }

    private AudioClipInfo GetSoundInfo(SoundName soundName)
    {
        foreach (AudioClipInfo soundAudioClip in audioClipInfoArray)
        {
            if (soundAudioClip.soundName == soundName)
            {
                return soundAudioClip;
            }
        }
        return null;
    }

    private void Awake()
    {
        if (sm != null)
        {
            Debug.LogWarning("More than 1 SoundManager in the scene");
            return;
        }
        sm = this;

        audioSource = GetComponent<AudioSource>();

        Dictionary<SoundName, List<AudioClipInfo>> tempDict = new();

        foreach (AudioClipInfo audioClipInfo in audioClipInfoArray)
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
}
