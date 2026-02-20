using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerUpSkill", menuName = "CreateSO/PowerUpSkillSO")]
public class PowerUpSkillSO : SkillSO
{
    public override Skill CreateSkill()
    {
        return new PowerUp();
    }
}
