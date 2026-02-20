using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealSkill", menuName = "CreateSO/HealSkillSO")]
public class HealSkillSO : SkillSO
{
    public override Skill CreateSkill()
    {
        return new Heal();
    }
}
