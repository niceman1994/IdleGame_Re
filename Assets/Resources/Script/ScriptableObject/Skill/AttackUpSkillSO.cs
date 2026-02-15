using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackUpSkill", menuName = "CreateSO/AttackUpSkillSO")]
public class AttackUpSkillSO : SkillSO
{
    public override Skill CreateSkill()
    {
        return new AttackUpSkill();
    }
}
