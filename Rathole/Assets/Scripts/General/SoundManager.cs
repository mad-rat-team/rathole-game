using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundInfo[] soundInfoArray;

    private static SoundManager sm;

    private AudioSource audioSource;

    public enum Sound
    {
        DoorOpen,
        DoorClose
    }

    [System.Serializable]
    public class SoundInfo
    {
        public Sound sound;
        public AudioClip audioClip;
        public float volume;
    }

    public static void PlaySoundEffect(Sound sound)
    {
        SoundInfo soundInfo = sm.GetSoundInfo(sound);
        sm.audioSource.PlayOneShot(soundInfo.audioClip, soundInfo.volume);
    }

    private SoundInfo GetSoundInfo(Sound sound)
    {
        foreach(SoundInfo soundAudioClip in soundInfoArray)
        {
            if(soundAudioClip.sound == sound)
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
    }
}
