using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillSO : ScriptableObject
{
    public string skillName;
    public float coolTimeReset;
    public float buffTime;
    public float skillPower;
    public Sprite skillImage;
    public AudioClip skillSoundClip;

    public abstract Skill CreateSkill();
}
