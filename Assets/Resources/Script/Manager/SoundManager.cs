using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CastSoundInfo
{
    public string soundName;
    public AudioClip audioClip;
}

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] AudioSource playerUIClickSound;
    [SerializeField] AudioSource upgradeStatClickSound;
    public CastSoundInfo[] castSounds;

    public void PlayPlayerUIClickSound()
    {
        playerUIClickSound.Play();
    }

    public void PlayUpgradeStatClickSound()
    {
        upgradeStatClickSound.Play();
    }
}
