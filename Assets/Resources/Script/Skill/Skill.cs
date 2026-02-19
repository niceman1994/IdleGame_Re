using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill
{
    protected Buff buff;
    public event Action onSkillSoundPlay;

    public abstract void AddBuff(SkillSO skillData);
    public virtual void Use(IObject target)
    {
        onSkillSoundPlay?.Invoke();
    }
}
