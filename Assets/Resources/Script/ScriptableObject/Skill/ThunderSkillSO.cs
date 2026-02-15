using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ThunderSkill", menuName = "CreateSO/ThunderSkillSO")]
public class ThunderSkillSO : SkillSO
{
    public override Skill CreateSkill()
    {
        return new ThunderSkill();
    }
}
