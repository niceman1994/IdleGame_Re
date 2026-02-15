using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderSkill : Skill
{
    public override void AddBuff(SkillSO skillData)
    {
        GameManager.Instance.PrepareThunder();
    }

    public override void Use(IObject target)
    {
        GameManager.Instance.SummonThunder();
    }
}
