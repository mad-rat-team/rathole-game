using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSoundEffectMapping", menuName = "Custom/Sound Effect Mapping")]
public class SoundEffectMapping : ScriptableObject
{
    public AudioClipInfo[] audioClipInfoArray;
    //public List<AudioClipInfo> test;
    //public AudioClipInfo tests;
}
