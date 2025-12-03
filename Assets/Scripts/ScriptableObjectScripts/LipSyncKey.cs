using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PhonemeKey
{
    [Tooltip("Seconds from the start of the audio.")]
    public float time;
    public Phoneme phoneme;
}

[Serializable]
public class PhonemeKeyData
{
    public List<PhonemeKey> keys;
}

[CreateAssetMenu(fileName = "NewLipSyncClip", menuName = "LipSync/Clip")]
public class LipSyncClip : ScriptableObject
{
    public AudioClip audio;
    public TextAsset jsonFile;
    public PhonemeKey[] keys;
}
