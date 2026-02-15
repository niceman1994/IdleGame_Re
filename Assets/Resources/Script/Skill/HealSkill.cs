using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSkill : Skill
{
    public override void AddBuff(SkillSO skillData)
    {
        buff = new DurationBuff(new HealBuff(skillData.skillPower), skillData.buffTime);
        //buffList.Add(buff);
    }

    public override void Use(IObject target)
    {
        buff.Apply(target);
        BuffManager.Instance.AddBuff((DurationBuff)buff);
    }
}
