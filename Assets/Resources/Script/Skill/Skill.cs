using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill
{
    protected Buff buff;

    public abstract void AddBuff(SkillSO skillData);
    public abstract void Use(IObject target);
}
