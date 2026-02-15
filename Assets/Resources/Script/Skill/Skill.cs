using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill
{
    protected Buff buff;

    public abstract void AddBuff(SkillSO skillData);
    public abstract void Use(IObject target);
    public virtual void CheckBuffTime()
    {
        if (buff != null && buff.IsActive)
            buff.Tick(Time.deltaTime);
    }
}
