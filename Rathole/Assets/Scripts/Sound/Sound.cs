using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum SoundName
{
    DoorOpen = 0,
    DoorClose = 1,
    KeyTwist = 2,
    Footstep = 3,
    NoteOpen = 4,
    PageTurn = 5,
    NoteClose = 6,
    PipeSwing = 7,
    Hit = 8,
    ItemPickUp = 9,
    SaveTotem = 10,
    DamageGrunt = 11,
    SoundtrackMain = 1000
}

[System.Serializable]
public class AudioClipInfo
{
    public SoundName soundName;
    public AudioClip audioClip;
    public float volume;
}

public abstract class Sound
{
    public abstract AudioClipInfo GetAudioClipInfo();
}

public class SimpleSound : Sound
{
    private AudioClipInfo audioClipInfo;

    public SimpleSound(AudioClipInfo audioClipInfo)
    {
        this.audioClipInfo = audioClipInfo;
    }

    public override AudioClipInfo GetAudioClipInfo()
    {
        return audioClipInfo;
    }
}

public class RandomSound : Sound
{
    private AudioClipInfo[] audioClips;
    private int lastClipIndex;
    public bool repeatingClipsAllowed;

    public RandomSound(AudioClipInfo[] audioClips, bool repeatingClipsAllowed)
    {
        this.audioClips = audioClips;
        this.repeatingClipsAllowed = repeatingClipsAllowed;
        lastClipIndex = 0;
    }

    public override AudioClipInfo GetAudioClipInfo()
    {
        if (repeatingClipsAllowed)
        {
            return audioClips[Random.Range(0, audioClips.Length)];
        }

        int index = Random.Range(0, audioClips.Length - 1);
        if (index >= lastClipIndex)
        {
            index++;
        }

        lastClipIndex = index;

        return audioClips[index];
    }
}
