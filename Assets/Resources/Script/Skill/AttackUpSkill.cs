using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUpSkill : Skill
{
    public override void AddBuff(SkillSO skillData)
    {
        buff = new InstantBuff(new AttackBuff(skillData.skillPower), skillData.buffTime);
    }

    public override void Use(IObject target)
    {
        base.Use(target);
        buff.Apply(target);
        target.AddBuff(buff);
    }
}
