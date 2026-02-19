using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAudioSound : MonoBehaviour
{
    [SerializeField] SkillController skillController;
    [SerializeField] AudioSource thunderAudio;

    private void Start()
    {
        skillController.onSkillSoundRequested += PlaySkillSound;
    }

    private void PlaySkillSound(SkillSO data)
    {
        if (data.skillSoundClip != null)
        {
            thunderAudio.clip = data.skillSoundClip;
            thunderAudio.Play();
        }
    }
}
