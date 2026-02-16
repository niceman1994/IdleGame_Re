using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    public void RegisterButton(SkillButton button)
    {
        button.onSkillPressed += HandleSkill;
    }

    private void HandleSkill(SkillSO data)
    {
        var skill = data.CreateSkill();
        skill.AddBuff(data);
        skill.Use(GetComponent<IObject>());
    }
}
