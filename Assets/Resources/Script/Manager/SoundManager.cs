using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SoundName
{
    public string soundName;
    public AudioClip audioClip;
}

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource buttonSound;
    public AudioSource upgradeSound;
    public SoundName[] castSounds;
}
